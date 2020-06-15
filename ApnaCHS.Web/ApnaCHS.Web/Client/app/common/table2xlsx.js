; (function ($, window, document, undefined) {
    var pluginName = "table2xlsx",

    defaults = {
        exclude: ".noExl",
        name: "Table2Xlsx",
        filename: "table2xlsx",
        fileext: ".xlsx",
        exclude_img: true,
        exclude_links: true,
        exclude_inputs: true
    };

    // The actual plugin constructor
    function Plugin(element, options) {
        var tables = [];

        $(element).find("table").each(function () {
            tables.push(this);
        });

        var data = [];
        angular.forEach(tables, function (v) {
            data = tableToJson(v);
        })

        this.element = data;
        // jQuery has an extend method which merges the contents of two or
        // more objects, storing the result in the first object. The first object
        // is generally empty as we don't want to alter the default options for
        // future instances of the plugin
        //
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {
        init: function () {
            var e = this;

            var wb = new Workbook(),
                ws = sheet_from_array_of_arrays(e.element);

            /* add worksheet to workbook */
            wb.SheetNames.push(e.settings.sheetName);
            wb.Sheets[e.settings.sheetName] = ws;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), e.settings.filename);
        }
    };

    function sheet_from_array_of_arrays(data, opts) {
        var ws = {};
        var range = { s: { c: 10000000, r: 10000000 }, e: { c: 0, r: 0 } };
        for (var R = 0; R != data.length; ++R) {
            for (var C = 0; C != data[R].length; ++C) {
                if (range.s.r > R) range.s.r = R;
                if (range.s.c > C) range.s.c = C;
                if (range.e.r < R) range.e.r = R;
                if (range.e.c < C) range.e.c = C;
                var cell = { v: data[R][C] };
                if (cell.v == null) continue;
                var cell_ref = XLSX.utils.encode_cell({ c: C, r: R });

                if (typeof cell.v === 'number') cell.t = 'n';
                else if (typeof cell.v === 'boolean') cell.t = 'b';
                else if (cell.v instanceof Date) {
                    cell.t = 'n';
                    cell.z = XLSX.SSF._table[14];
                    cell.v = datenum(cell.v);
                }
                else cell.t = 's';

                if (R == 0) {
                    cell.s = {
                        font: {
                            bold: true
                        },
                        border: {
                            top: { style: "thin" },
                            bottom: { style: "thin" },
                            left: { style: "thin" },
                            right: { style: "thin" },
                        }
                    }
                }
                else {
                    cell.s = {
                        border: {
                            top: { style: "thin" },
                            bottom: { style: "thin" },
                            left: { style: "thin" },
                            right: { style: "thin" },
                        }
                    }
                }

                ws[cell_ref] = cell;
            }
        }
        if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
        return ws;
    }

    function Workbook() {
        if (!(this instanceof Workbook)) return new Workbook();
        this.SheetNames = [];
        this.Sheets = {};
    }

    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    function tableToJson(table) {
        var data = []; // first row needs to be headers
        var header = [];

        for (var i = 0; i < table.rows[0].cells.length; i++) {
            header.push(table.rows[0].cells[i].innerHTML);
        }
        data.push(header);

        // go through cells 
        for (var i = 1; i < table.rows.length; i++) {

            var tableRow = table.rows[i];
            var rowData = [];
            for (var j = 0; j < tableRow.cells.length; j++) {
                rowData.push(tableRow.cells[j].innerHTML);
            }

            data.push(rowData);
        }

        return data;
    }


    $.fn[pluginName] = function (options) {
        var e = this;
        e.each(function () {
            if (!$.data(e, "plugin_" + pluginName)) {
                $.data(e, "plugin_" + pluginName, new Plugin(this, options));
            }
        });

        // chain jQuery functions
        return e;
    };

})(jQuery, window, document);

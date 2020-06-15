app.directive('commentbox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', 'authService',
    function ($http, DATA_URLS, $location, $anchorScroll, authService) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/comment/commentBox.html',
            replace: true,
            transclude: true,
            scope: {
                formData: '=',
                table: '='
            },
            link: function (scope, element, attrs, location) {

                scope.tables =
                {
                    "MC": { "add": DATA_URLS.COMMENTMC_ADD, "update": DATA_URLS.COMMENTMC_UPDATE },
                    "Flat": { "add": DATA_URLS.COMMENTFLAT_ADD, "update": DATA_URLS.COMMENTFLAT_UPDATE },
                    "FlatOwner": { "add": DATA_URLS.COMMENTFLATOWNER_ADD, "update": DATA_URLS.COMMENTFLATOWNER_UPDATE },
                    "FlatOwnerFamily": { "add": DATA_URLS.COMMENTFLATOWNERFAMILY_ADD, "update": DATA_URLS.COMMENTFLATOWNERFAMILY_UPDATE },
                    "Vehicle": { "add": DATA_URLS.COMMENTVEHICLE_ADD, "update": DATA_URLS.COMMENTVEHICLE_UPDATE },
                };
                scope.currentUserName = authService.authentication.loginData.userName;

                clearComment();

                scope.$watch("formData.comments", function (newValue, oldValue) {
                    if (newValue != oldValue) {
                        sortArray();
                    }
                });

                function sortArray() {
                    if (!!scope.formData.comments) {

                        var result = scope.formData.comments.sort(function (x, y) {
                            return (x.createdDate === y.createdDate) ? 0 : x.createdDate ? -1 : 1;
                        });

                        scope.formData.comments = result;
                    }
                }

                scope.addNewCommentClicked = function () {
                    scope.isAddNewComment = true;
                }

                scope.addNewCommentCancelClicked = function () {
                    clearComment()
                }

                scope.addNewComment = function () {

                    if (scope.table == "MC") {
                        scope.commentFormData.maintenanceCostId = scope.formData.id;
                    }
                    else if (scope.table == "Flat") {
                        scope.commentFormData.flatId = scope.formData.id;
                    }
                    else if (scope.table == "FlatOwner") {
                        scope.commentFormData.flatOwnerId = scope.formData.id;
                    }
                    else if (scope.table == "FlatOwnerFamily") {
                        scope.commentFormData.flatOwnerFamilyId = scope.formData.id;
                    }
                    else if (scope.table == "Vehicle") {
                        scope.commentFormData.vehicleId = scope.formData.id;
                    }

                    var promise = $http.post(scope.tables[scope.table].add, scope.commentFormData);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.formData.comments.unshift(data);
                            //scope.formData.comments.push(data);
                            clearComment();
                        }
                    });
                }

                function clearComment() {
                    scope.isAddNewComment = false;
                    scope.commentFormData = {};
                }

                scope.getCommentTemplate = function (comment) {
                    return (!!scope.editingComment && scope.editingComment.id == comment.id) ? 'commentEdit' : 'commentDisplay';
                }

                scope.editComment = function (comment) {
                    comment.isEdit = true;
                }

                scope.cancelEditClicked = function (comment) {
                    comment.isEdit = false;
                }

                scope.updateComment = function (index) {

                    var comment = { id: scope.formData.comments[index].id, text: scope.formData.comments[index].text };
                    var promise = $http.post(scope.tables[scope.table].update, comment);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            scope.formData.comments[index].text = comment.text;
                            scope.cancelEditClicked(scope.formData.comments[index]);
                        }
                    });
                }

                scope.getLetter = function (text) {
                    return !text ? 'a' : text.substring(0, 1);
                }
            }
        };
    }]);
angular.module("umbraco").controller("TwoFactor.PermissionsDashboard",
    function ($scope, adminGroupService, $route) {
        $scope.error = '';

        adminGroupService.getGroups()
            .then(function (response) {
                $scope.groups = response.data;
            }, function () {
                $scope.error += '<br />Could not load group data';
            });
        $scope.save = function () {
            adminGroupService.saveGroups($scope.groups)
                .then(function () {
                    $route.reload();
                }, function () {
                    $scope.error += '<br />Could not save changes';
                });
        }
    }
);
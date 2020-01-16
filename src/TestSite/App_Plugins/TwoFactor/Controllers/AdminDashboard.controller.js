angular.module("umbraco").controller("TwoFactor.AdminDashboard",
    function ($scope, adminUserService, $route, $location) {
        $scope.error = '';

        adminUserService.pages()
            .then(function (response) {
                $scope.totalPages = response.data;
                $scope.pages = [];
                for (var i = 0; i < $scope.totalPages; i++) {
                    $scope.pages.push(i + 1);
                }
            }, function () {
                $scope.error += '<br />Could not load page info';
            });

        $scope.currentPage = $location.search().page;
        if ($scope.currentPage == null || $scope.currentPage < 0 || $scope.currentPage > $scope.totalPages) {
            $scope.currentPage = 1;
        }

        adminUserService.getTwoFactor($scope.currentPage)
            .then(function (response) {
                $scope.users = response.data;
            }, function () {
                $scope.error += '<br />Could not load user data';
            });

        $scope.deleteDevices = function () {
            adminUserService.deleteTwoFactor($scope.users)
                .then(function () {
                    refresh();
                }, function () {
                    $scope.error += `<br />Could not reset 2fa of user ${userId}`;
                });
        };

        $scope.goTo = function (page) {
            $route.updateParams({ page: page });
        };

        $scope.next = function () {
            if ($scope.currentPage < $scope.totalPages) {
                $scope.goTo($scope.currentPage + 1);
            }
        };

        $scope.previous = function () {
            if ($scope.currentPage > 1) {
                $scope.goTo($scope.currentPage - 1);
            }
        };

        function refresh() {
            $route.reload();
        }
    }
);

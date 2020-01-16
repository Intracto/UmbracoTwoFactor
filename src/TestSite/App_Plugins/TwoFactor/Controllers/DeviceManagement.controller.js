angular.module("umbraco").controller("TwoFactor.DeviceManagement",
    function ($scope, deviceService, $timeout, twoFactorUserService, Constants) {
        $scope.error = '';

        //getting all enabled data
        twoFactorUserService.anyEnabled()
            .then(function (response) {
                $scope.anyEnabled = response.data;
            }, function () {
                $scope.error += '<br />Could not load AnyEnabled'
            });
        twoFactorUserService.typeEnabled(Constants.Totp)
            .then(function (response) {
                $scope.totpEnabled = response.data;
                refresh();
            }, function () {
                $scope.error += '<br />Could not load TotpEnabled'
            });
        twoFactorUserService.typeEnabled(Constants.Mail)
            .then(function (response) {
                $scope.mailEnabled = response.data;
            }, function () {
                $scope.error += '<br />Could not load MailEnabled'
            });
            
        //getting all allowed data
        twoFactorUserService.anyAllowed()
            .then(function (response) {
                $scope.anyAllowed = response.data;
            }, function () {
                $scope.error += '<br />Could not load AnyAllowed'
            });
        twoFactorUserService.typeAllowed(Constants.Totp)
            .then(function (response) {
                $scope.totpAllowed = response.data;
            }, function () {
                $scope.error += '<br />Could not load TotpAllowed'
            });
        twoFactorUserService.typeAllowed(Constants.Mail)
            .then(function (response) {
                $scope.mailAllowed = response.data;
            }, function () {
                $scope.error += '<br />Could not load MailAllowed'
            });

        function onTimeout() {
            $scope.counter--;
            if ($scope.counter > 0) {
                for (var i = $scope.devices.length - 1; i >= 0; i--) {
                    $scope.devices[i].RemainingSeconds = $scope.counter;
                }
                $timeout(onTimeout, 1000);
            }
            else {
                refresh();
            }
        }

        function refresh() {
            deviceService.getTwoFactorModels()
                .then(function (response) {
                    $scope.devices = response.data;
                    $scope.counter = $scope.devices[0].RemainingSeconds;
                    $timeout(onTimeout, 1000);
                }, function () {
                    $scope.error += '<br />Could not load devices';
                });
        };

        $scope.deleteDevices = function () {
            deviceService.deleteDevices($scope.devices)
                .then(function () {
                    $timeout.cancel(onTimeout)
                        .then(function () {
                            refresh();
                        });
                }, function () {
                    $scope.error += '<br />Could not delete devices'
                });
        };

        $scope.disableMail = function () {
            deviceService.disableMail()
                .then(function () {
                    $scope.mailEnabled = false;
                }, function () {
                    $scope.error += '<br />Could not disable mail'
                });
        };
    }
);
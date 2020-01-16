angular.module("umbraco").controller("TwoFactor.RegisterDashboard",
    function ($scope, registerService, twoFactorUserService, $route, Constants) {
        $scope.totpCode = "";
        $scope.error = '';
        $scope.addingDevice = false;

        registerService.generateSecret()
            .then(function (response) {
                $scope.secret = response.data;
                registerService.generateQR(response.data)
                    .then(function (qrResponse) {
                        $scope.mail = qrResponse.data.Mail;
                        $scope.applicationName = qrResponse.data.ApplicationName;
                        $scope.QRLoaded = true;
                    }, function () {
                        $scope.error += '<br />Could not load QRCode'
                    });
            }, function () {
                $scope.error += '<br />Could not generate Secret'
            });

        //getting all enabled data
        twoFactorUserService.anyEnabled()
            .then(function (response) {
                $scope.anyEnabled = response.data;
            }, function () {
                $scope.error += '<br />Could not load AnyEnabled'
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

        //save methods for the view
        $scope.validateTotpAndSave = function (code) {
            $scope.correctCode = null;
            registerService.validateTotp(code, $scope.secret)
                .then(function (response) {
                    if (response.data === true) {
                        registerService.saveTwoFactor(Constants.Totp, $scope.secret);
                        $route.reload();
                    } else {
                        $scope.correctCode = false;
                    }
                }, function () {
                    $scope.correctCode = false;
                });
        };

        $scope.saveMail = function () {
            registerService.saveTwoFactor(Constants.Mail, $scope.secret);
            $route.reload();
        }

        $scope.addTotpOrCancel = function () {
            $scope.addingDevice = !$scope.addingDevice;
        }
    }
);

angular.module("umbraco").controller("TwoFactor.ForceRegister",
    function ($scope, publicRegisterService, $window, Constants, authResource) {
        $scope.totpCode = "";
        $scope.mailEnabled = false;
        $scope.error = '';

        publicRegisterService.generateSecret()
            .then(function (response) {
                $scope.secret = response.data;
                publicRegisterService.generateQR(response.data)
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

        //getting all allowed data
        publicRegisterService.typeAllowed(Constants.Totp)
            .then(function (response) {
                $scope.totpAllowed = response.data;
            }, function () {
                $scope.error += '<br />Could not load TotpAllowed'
            });
        publicRegisterService.typeAllowed(Constants.Mail)
            .then(function (response) {
                $scope.mailAllowed = response.data;
            }, function () {
                $scope.error += '<br />Could not load MailAllowed'
            });

        $scope.validateTotpAndSave = function (code) {
            $scope.correctTotpCode = null;
            publicRegisterService.validateTotp(code, $scope.secret)
                .then(function (response) {
                    if (response.data === true) {
                        publicRegisterService.saveTwoFactor(Constants.Totp, $scope.secret)
                            .then(function () {
                                authResource.verify2FACode(Constants.Totp, code)
                                    .then(function () {
                                        login();
                                    });
                            });
                    } else {
                        $scope.correctTotpCode = false;
                    }
                }, function () {
                    $scope.correctTotpCode = false;
                });
        };

        $scope.enableMail = function () {
            publicRegisterService.saveTwoFactor(Constants.Mail, $scope.secret)
                .then(function () {
                    $scope.mailEnabled = true;
                });
        }

        $scope.sendMail = function () {
            if ($scope.mailEnabled) {
                authResource.send2FACode(Constants.Mail)
                    .then(function () {
                        $scope.mailSend = true;
                    });
            }
        }

        $scope.validateMail = function (code) {
            $scope.correctMailCode = null;
            authResource.verify2FACode(Constants.Mail, code)
                .then(function () {
                    login();
                }, function () {
                    $scope.correctMailCode = false;
                });
        }

        function login() {
            $window.location.href = Constants.AdminURL;
        }
    }
);
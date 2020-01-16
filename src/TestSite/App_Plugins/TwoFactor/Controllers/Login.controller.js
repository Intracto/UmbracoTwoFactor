angular.module("umbraco").controller("TwoFactor.Login",
    function ($scope, userService, authResource, $window, Constants, cookieService, publicCookieService) {
        $scope.totpCode = "";
        $scope.mailCode = "";
        $scope.needsAuth = false;

        publicCookieService.hasAuthCookie()
            .then(function (response) {
                if (response.data) {
                    $window.location.href = Constants.AdminURL;
                } else {
                    $scope.needsAuth = true;
                }
            });

        authResource.get2FAProviders()
            .then(function (response) {
                $scope.totpEnabled = response.includes(Constants.Totp)
                $scope.mailEnabled = response.includes(Constants.Mail);
            });

        $scope.validateTotp = function (code) {
            $scope.correctTotpCode = null;
            authResource.verify2FACode(Constants.Totp, code)
                .then(function (data) {
                    login(data);
                },
                    // wrong code
                    function () {
                        $scope.correctTotpCode = false
                    });
        };

        $scope.validateMail = function (code) {
            $scope.correctMailCode = null;
            authResource.verify2FACode(Constants.Mail, code)
                .then(function (data) {
                    login(data);
                },
                    // wrong code
                    function () {
                        $scope.correctMailCode = false
                    });
        };

        $scope.sendMail = function () {
            authResource.send2FACode(Constants.Mail)
                .then(function () {
                    $scope.mailSend = true;
                });
        };

        function login(data) {
            userService.setAuthenticationSuccessful(data);
            cookieService.setCookie()
                .then(function () {
                    $window.location.href = Constants.AdminURL;
                })
        }
    }
);

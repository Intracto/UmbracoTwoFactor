angular.module("umbraco").directive("qrGen", function () {
    return {
        link: link
    };
    function link(scope, element) {
        new QRCode(element[0], "otpauth://totp/" + scope.mail + "?secret=" + scope.secret + "&issuer=" + scope.applicationName)
    }
});

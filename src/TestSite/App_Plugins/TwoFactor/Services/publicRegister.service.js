angular.module("umbraco.services").factory("publicRegisterService", function ($http) {
    return {
        generateSecret: function() {
            return $http.get(`/umbraco/2fa/PublicRegister/GenerateSecret`);
        },
        generateQR: function(secret) {
            return $http.get(`/umbraco/2fa/PublicRegister/GenerateQR?secret=${secret}`);
        },
        generateNewQR: function () {
            return $http.get(`/umbraco/2fa/PublicRegister/GenerateQR`);
        },
        validateTotp: function (code, secret) {
            return $http.post( `/umbraco/2fa/PublicRegister/ValidateTotp?code=${code}&secret=${secret}`);
        },
        saveTwoFactor: function (type, secret) {
            return $http.post(`/umbraco/2fa/PublicRegister/SaveTwoFactor?type=${type}&secret=${secret}`);
        },
        typeAllowed: function (type) {
            return $http.get(`/umbraco/2fa/PublicRegister/TypeAllowed?type=${type}`);
        }
    };
});
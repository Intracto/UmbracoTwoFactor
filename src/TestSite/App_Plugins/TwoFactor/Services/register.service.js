angular.module("umbraco.services").factory("registerService", function($http) {
    return {
        generateSecret: function() {
            return $http.get(`/umbraco/backoffice/2fa/Register/GenerateSecret`);
        },
        generateQR: function(secret) {
            return $http.get(`/umbraco/backoffice/2fa/Register/GenerateQR?secret=${secret}`);
        },
        generateNewQR: function() {
            return $http.get(`/umbraco/backoffice/2fa/Register/GenerateQR`);
        },
        validateTotp: function(code, secret) {
            return $http.post(`/umbraco/backoffice/2fa/Register/ValidateTotp?code=${code}&secret=${secret}`);
        },
        saveTwoFactor: function(type, secret) {
            return $http.post(`/umbraco/backoffice/2fa/Register/SaveTwoFactor?type=${type}&secret=${secret}`);
        }
    };
});
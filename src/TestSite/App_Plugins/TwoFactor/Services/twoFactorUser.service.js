angular.module("umbraco.services").factory("twoFactorUserService", function($http) {
    return {
        anyEnabled: function() {
            return $http.get(`/umbraco/backoffice/2fa/User/AnyEnabled`);
        },
        typeEnabled: function(type) {
            return $http.get(`/umbraco/backoffice/2fa/User/TypeEnabled?type=${type}`);
        },
        anyAllowed: function() {
            return $http.get(`/umbraco/backoffice/2fa/User/AnyAllowed`);
        },
        typeAllowed: function(type) {
            return $http.get(`/umbraco/backoffice/2fa/User/TypeAllowed?type=${type}`);
        }
    };
});

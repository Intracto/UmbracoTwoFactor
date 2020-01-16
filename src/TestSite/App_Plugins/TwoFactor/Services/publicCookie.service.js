angular.module("umbraco.services").factory("publicCookieService", function($http) {
    return {
        hasAuthCookie: function() {
            return $http.post(`/umbraco/2fa/PublicCookie/HasAuthCookie`);
        }
    };
});
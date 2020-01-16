angular.module("umbraco.services").factory("adminGroupService", function($http) {
    return {
        getGroups: function() {
            return $http.get(`/umbraco/backoffice/2fa/AdminGroup/GetGroups`);
        },
        saveGroups: function(groups) {
            return $http.post(`/umbraco/backoffice/2fa/AdminGroup/SaveGroups?input=${JSON.stringify(groups)}`);
        }
    };
});

angular.module("schoolApp.services").
       factory("peopleService", function ($resource) {
           return $resource(
               "/api/people/:Id",
               { Id: "@Id" },
               { "update": { method: "PUT" } }
          );
       });
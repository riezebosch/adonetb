
angular.module('schoolApp').controller('peopleController', function ($scope, peopleService) {
    $scope.loading = true;
    $scope.editMode = false;

    peopleService.query(function (data) {
        $scope.people = data;
        $scope.loading = false;
    },
    function () {
        $scope.error = "An error has occured while loading data!";
        $scope.loading = false;
    });

    $scope.toggleEdit = function () {
        $scope.editMode = !$scope.editMode;
    };

    $scope.save = function (person) {
        peopleService.update({ Id: person.personID }, person, function () {
            alert("Saved successfully!!");
        },
        function (data) {
            console.error(data);
            $scope.error = "An error has occured while saving changes! " + data;
            $scope.loading = false;
        });
    };
});
window.doAlert = function (message) {
    alert(message);

    DotNet.invokeMethodAsync('PeopleManager.Ui.BlazorApp', 'HideButton');
}
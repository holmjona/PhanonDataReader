var modules = {};
$.ajax({
    // this file needs contain all exercises from the Progress Map Page
    url: "../data/modules.json"
    , success: function (data) {
        modules = data.phanon_modules;
    }
});


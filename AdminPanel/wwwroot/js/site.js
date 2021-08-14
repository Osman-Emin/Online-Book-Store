// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    function ConfirmDialog(message) {
        var def = $.Deferred();
        var result = false;
        $('<div></div>').appendTo('body')
            .html('<div><h6>' + message + '?</h6></div>')
            .dialog({
                modal: true,
                title: 'Confirm',
                zIndex: 10000,
                autoOpen: true,
                width: 'auto',
                resizable: false,
                buttons: {
                    Yes: function() {
                        result = true;

                        $(this).dialog("close");
                    },
                    No: function() {
                        result = false;

                        $(this).dialog("close");
                    }
                },
                close: function(event, ui) {
                    $(this).remove();
                    if(result) 
                        def.resolve();
                    else def.reject();
                }
            });
        // create and/or show the dialog box here
        // but in "OK" do 'def.resolve()'
        // and in "cancel" do 'def.reject()'

        return def.promise();
    
}
function InfoDialog(message) {
    
        $('<div></div>').appendTo('body')
            .html('<div><h6>' + message + '?</h6></div>')
            .dialog({
                modal: true,
                title: 'Info',
                zIndex: 10000,
                autoOpen: true,
                width: 'auto',
                resizable: false,
                buttons: {
                    Ok: function() {
                        $(this).dialog("close");
                    }
                },
                close: function(event, ui) {
                    $(this).remove();
                }
            });
}

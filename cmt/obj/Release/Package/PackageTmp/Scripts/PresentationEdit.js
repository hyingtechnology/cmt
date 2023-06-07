var PresentationEdit = {

    PresentationPreview: function () {
        
        var Title = $('td[isTitle]').find('input').val();
        var Content = tinymce.activeEditor.getContent();

        var o = new Object();
        o.Title = Title;
        o.Content = Content;

        $.ajax({
            url: PresentationPreview,
            type: "POST",
            data: o,
            success: function (response) {
                window.open(PresentationPreview, '_blank');
               // location.href = PresentationPreview;
            },
            error: function () {
                alert('error');
            }
        });
    }
}
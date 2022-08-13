const { type } = require("jquery");

var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('#name');
            var phone = $('#phone');
            var email = $('#');
            var address = $('#');
            var content = $('#');

            $.ajax({
                url: '/Contact/Feedback',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    phone: phone,
                    email: email,
                    address: address,
                    content: content,
                },
                success: function (res) {
                    if (res.status==true) {

                    };
                }
            });
        });
    }
}
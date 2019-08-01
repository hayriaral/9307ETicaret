$(document).ready(function () {
    $("#js-add-member-form").validate({

        rules: {
            //girilen her nesneden sonra  virgül atılması gerekir.
            FirstName: "required", //tek satır kontrol ediyoruz. girilmesi zorunlu alan olarak belirttik.

            LastName: {
                required: true
                // bu alanda aynı işlevi görüyor scope açıldıgı için birden fazla koşul gireblinir. aralarına virgül konarak.
            },

            Email:{
                required: true,
                email: true //email formatinda alıyoruz.
            },

            Password: {
                required: true,
                rangelength: [8,16]
            },

            confirm:{
                equalTo:"#js-input-password"
            }
        }
    });
});
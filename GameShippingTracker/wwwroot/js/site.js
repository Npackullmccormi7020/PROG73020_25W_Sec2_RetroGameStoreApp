$(document).ready(function () {
    var _shippingsUrl = 'https://localhost:7200/api/Shipping';
    var newShippingsUrl;

    var _shippingItemsList = $('#shippingItemsList');

    var _shippingItemsMsg = $('#shippingItemsMsg');

    var getDateString = function (dt) {
        return `${dt.getMonth() + 1}/${dt.getDate()}/${dt.getFullYear()}`;
    };

    var loadShippings = async function () {
        // setup the GET request for the tasks from the API:
        const resp = await fetch(_shippingsUrl, {
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        });

        // get the JSON body as an array of shipping objects:
        const shippings = await resp.json();

        if (resp.status === 200) {
            if (shippings.length > 0) {
                // clear the list of shippings first:
                _shippingItemsList.empty();

                // loop thru the shippingss and append them to the list:
                for (let i = 0; i < shippings.length; i++) {
                    _shippingItemsList.append('<li> Shipping Id: ' + shippings[i].shippingId + ' Status: ' +
                        shippings[i].isShipped + ' sent on ' + getDateString(new Date(shippings[i].shippedDate)) + '</li>');
                }
            } else {
                _shippingItemsMsg.text('Currently no shippings - use the form to add some!');
            }
        } else {
            _shippingItemsMsg.text('Hmmm, there was a problem retrieving shippings from the server.');
        }
    };



    //$('#shippingSubmit').click(async function () {
    //    var shippingText = document.getElementById("shippingIdField").value;
    //    newShippingsUrl = _shippingsUrl + "/" + shippingText;
    //    loadShippings();
    //});

    // first load the shipings:
    loadShippings();

    // and re-load every 1 sec:
    setInterval(loadShippings, 1000);
});
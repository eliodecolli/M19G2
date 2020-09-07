/**
 * @param {any} url URL expecting ajax call
 * @param {any} dataType The type of data expected back from server
 * @param {any} contentType The type of content being posted to the server
 * @param {any} processData The flag pertaining to the processing of data
 * @param {any} requestData The request data [object] that is going to be sent to the server
 * @param {any} requestMethod The request method name
 * @param {any} preRequestCallBack The callback method to be executed before the ajax call
 * @param {any} postRequestCallback The callback method to be executed after the ajax call
 * @param {any} successCallback The callback method to be executed on ajax call success
 * @param {any} errorCallback The callback method to be executed on ajax call error
 */
function CallGenericAjax( // 
    url, // url ku postojem eshe CONTROLLERNAME/ACTIONNAME
    dataType = "json", 
    contentType = "application/json;", 
    processData = true,// PROCESS DATA, NE RAST KUR POSTOJME NJE JSON ARRAY OSE SERIALIZED DATA OSE WHATEVER, KTE E BEJME TRUE, NE SPOSTOJME GJE, KSHUQE E LEME FALSE
    requestData, // REQUEST DATA ESHTE THE DATA QE POSTOJME TEK ACTION/METHOD
    requestMethod = null, // REQUEST METHOD ESHTE OSE GET OSE POST, NE KEMI GET
    preRequestCallBack = null, 
    postRequestCallback = null,// THIS IS THE SAME, POyeRs EyEXsye AFTER SUBMIT OF REQ
    successCallback,// ERROR CALLACK KA GJITHASHTU TRE PARAMETRA, MUND TA LEME NULL, QE TE EKZEKUTOHET DEFAULT QE THJESHT LOGON NE CONSOLE, K DERI KTU?yes
    errorCallback = null) {
    if (preRequestCallBack !== null)
        preRequestCallBack();

    $.ajax({
        type: requestMethod,
        url: url,
        data: requestData,
        contentType: contentType,
        dataType: dataType,
        processData: processData,
        success: function (data, textStatus, request) {
            successCallback(data, textStatus, request);
            if (postRequestCallback !== null)
                postRequestCallback();

        },
        error: function (xhr, textStatus, errorThrown) {
            if (errorCallback === null)
                console.log(errorThrown);
            else
                errorCallback(xhr, textStatus, errorThrown);

            if (postRequestCallback !== null)
                postRequestCallback();
        }
    });
}
var ChartSet = {
    None: {
        name: 'none',
        containsChar: function (c) {
            return false;
        }
    }
}

function EncodeXML(aInput) {

    if (Configuration.PublishMode == "Device")
        return aInput;

    var Output = "";
    for (var i = 0; i < aInput.length; i++) {
        var j = '<>"&\''.indexOf(aInput.charAt(i));
        if (j != -1) {
            Output += '&' + ['lt', 'gt', 'quot', 'amp', '#39'][j] + ';';
        } else if (!ChartSet["None"].containsChar(aInput.charAt(i))) {
            Output += '&#' + aInput.charCodeAt(i) + ';';
        } else {
            Output += aInput.charAt(i);
        }
    }

    return Output;
}

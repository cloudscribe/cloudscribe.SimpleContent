jQuery.validator.addMethod("requiredwhen", function (value, element, param) {
    var otherPropId = $(element).data('val-other');
	var otherPropVal = $(element).data('val-otherval');
    if (otherPropId) {
        var otherProp = $(otherPropId);
        if (otherProp) {
            var otherVal = otherProp.val();
            console.log(otherVal);
            console.log(otherPropVal);
            return false;
            if (otherVal === otherPropVal) {
                console.log('check requied val');
                return element.value.length > 0;
            }
        }
    }
    return false;
});
jQuery.validator.unobtrusive.adapters.addBool("requiredwhen");

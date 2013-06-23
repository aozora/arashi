///* Client-side validation for equaltoproperty rule */
//jQuery.validator.addMethod("equaltoproperty", function (value, element, params) {
//   if (this.optional(element)) {
//      return true;
//   }

//   var otherPropertyControl = $("#" + params.otherproperty);
//   if (otherPropertyControl == null) {
//      return false;
//   }

//   var otherPropertyValue = otherPropertyControl[0].value;
//   return otherPropertyValue == value;
//});



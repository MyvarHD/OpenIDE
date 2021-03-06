/*!
 * Copyright (c) 2011 "Cowboy" Ben Alman
 * Dual licensed under the MIT and GPL licenses.
 * http://benalman.com/about/license/
 */

(function(exports, global) {
  // A map of basic Object.prototype.toString output to [[Class]] names. This
  // serves the dual purpose of being the object on which the Object.prototype
  // .toString method is called.
  var classMap = {
    "[object Number]": "Number",
    "[object String]": "String",
    "[object Boolean]": "Boolean",
    "[object Function]": "Function",
    "[object RegExp]": "RegExp",
    "[object Array]": "Array",
    "[object Date]": "Date",
    "[object Error]": "Error"
  };
  // Match "[object ___]" where "___" is a [[Class]] value.
  var className = /^\[object (.*)\]$/;

  // Return a specific useful value based on a passed object's [[Class]].
  exports.getClass = function(value, options) {
    // Initialize options argument if not truthy.
    if (!options) {
      options = {};
    }

    if (value === null) {
      // If the passed value is null, return "Null" (IE <= 8)
      return "Null";
    } else if (value == null) {
      // If the passed value is undefined (the only value that is == null but
      // !== null is undefined), return "Undefined" (IE <= 8)
      return "Undefined";
    } else if (options.specific && value === global) {
      // If in "specific" mode, and the passed value is the global object,
      // return "global"
      return "global";
    }

    // Get the "[object [[Class]]]" string for the passed value.
    var key = classMap.toString.call(value);

    if (classMap.hasOwnProperty(key)) {
      // If the key exists in classMap, return its [[Class]].
      return classMap[key];
    } else if (options.specific && className.test(key)) {
      // If in "specific" mode and key matches pattern, parse the [[Class]]
      // value from the string and return that.
      return className.exec(key)[1];
    } else {
      // If not in "specific" mode or key doesn't match pattern, just return
      // the more generic "Object".
      return "Object";
    }
  };
}(typeof exports === "object" && exports || this, this));
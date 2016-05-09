
(function() {
'use strict';

// This bookmarklet lists Polymer elements that have been registered
// but never used on the page. This is useful if you want know when
// you're loading extra HTML Imports that you don't need.

// Note: requires ES6 Set() and => functions.

function isCustomEl(el) {
  return el.localName.indexOf('-') != -1 || el.getAttribute('is');
}

let els = [].slice.call(document.querySelectorAll('html /deep/ *'))
    .filter(el => isCustomEl(el))
    .map(el => el.getAttribute('is') || el.localName);

let allCustomElements = new Set(els); // get unique names bro!

let polymerRegisteredElements = Polymer.telemetry.registrations
    .map(el => el.is)
    .filter(name => {
      let blacklist = ['dom-template', 'array-selector', 'custom-style'];
      return blacklist.indexOf(name) === -1;
    });

let diff = [];
for (let i = 0, name; name = polymerRegisteredElements[i]; ++i) {
  if (!allCustomElements.has(name)) {
    diff.push(name);
  }
}

if (diff.length) {
  console.info("There are unused HTML Imports on this page!");
  console.log("Check that you're not loading extra imports.\n" +
              "The following elements are registered but never used on the page.");

  diff.forEach(name => console.log(name));
}

})();

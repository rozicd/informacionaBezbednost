"use strict";
/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
(() => {
var exports = {};
exports.id = "pages/ActivationPage";
exports.ids = ["pages/ActivationPage"];
exports.modules = {

/***/ "./src/pages/ActivationPage.js":
/*!*************************************!*\
  !*** ./src/pages/ActivationPage.js ***!
  \*************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (/* binding */ ActivateAccount)\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"react/jsx-dev-runtime\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! next/router */ \"next/router\");\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(next_router__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react */ \"react\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_2__);\n\n\n\nfunction ActivateAccount() {\n    const router = (0,next_router__WEBPACK_IMPORTED_MODULE_1__.useRouter)();\n    const { id , token  } = router.query;\n    async function activateAccount() {\n        const response = await fetch(`/api/activate/${id}/${token}`, {\n            method: \"PUT\"\n        });\n        if (response.ok) {\n            // Account activated successfully\n            router.push(\"/success\");\n        } else {\n            // Error occurred during activation\n            const message = await response.text();\n            alert(`Failed to activate account: ${message}`);\n        }\n    }\n    (0,react__WEBPACK_IMPORTED_MODULE_2__.useEffect)(()=>{\n        activateAccount();\n    }, []);\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"h1\", {\n                children: \"Activate Your Account\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\ActivationPage.js\",\n                lineNumber: 28,\n                columnNumber: 7\n            }, this),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"p\", {\n                children: \"Activating your account, please wait...\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\ActivationPage.js\",\n                lineNumber: 29,\n                columnNumber: 7\n            }, this)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\ActivationPage.js\",\n        lineNumber: 27,\n        columnNumber: 5\n    }, this);\n}\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvcGFnZXMvQWN0aXZhdGlvblBhZ2UuanMuanMiLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7QUFBd0M7QUFDTjtBQUVuQixTQUFTRSxrQkFBa0I7SUFDeEMsTUFBTUMsU0FBU0gsc0RBQVNBO0lBQ3hCLE1BQU0sRUFBRUksR0FBRSxFQUFFQyxNQUFLLEVBQUUsR0FBR0YsT0FBT0csS0FBSztJQUVsQyxlQUFlQyxrQkFBa0I7UUFDL0IsTUFBTUMsV0FBVyxNQUFNQyxNQUFNLENBQUMsY0FBYyxFQUFFTCxHQUFHLENBQUMsRUFBRUMsTUFBTSxDQUFDLEVBQUU7WUFDM0RLLFFBQVE7UUFDVjtRQUNBLElBQUlGLFNBQVNHLEVBQUUsRUFBRTtZQUNmLGlDQUFpQztZQUNqQ1IsT0FBT1MsSUFBSSxDQUFDO1FBQ2QsT0FBTztZQUNMLG1DQUFtQztZQUNuQyxNQUFNQyxVQUFVLE1BQU1MLFNBQVNNLElBQUk7WUFDbkNDLE1BQU0sQ0FBQyw0QkFBNEIsRUFBRUYsUUFBUSxDQUFDO1FBQ2hELENBQUM7SUFDSDtJQUVBWixnREFBU0EsQ0FBQyxJQUFNO1FBQ2RNO0lBQ0YsR0FBRyxFQUFFO0lBRUwscUJBQ0UsOERBQUNTOzswQkFDQyw4REFBQ0M7MEJBQUc7Ozs7OzswQkFDSiw4REFBQ0M7MEJBQUU7Ozs7Ozs7Ozs7OztBQUdULENBQUMiLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9pYi1jbGllbnQvLi9zcmMvcGFnZXMvQWN0aXZhdGlvblBhZ2UuanM/NzM5OCJdLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyB1c2VSb3V0ZXIgfSBmcm9tICduZXh0L3JvdXRlcic7XHJcbmltcG9ydCB7IHVzZUVmZmVjdCB9IGZyb20gJ3JlYWN0JztcclxuXHJcbmV4cG9ydCBkZWZhdWx0IGZ1bmN0aW9uIEFjdGl2YXRlQWNjb3VudCgpIHtcclxuICBjb25zdCByb3V0ZXIgPSB1c2VSb3V0ZXIoKTtcclxuICBjb25zdCB7IGlkLCB0b2tlbiB9ID0gcm91dGVyLnF1ZXJ5O1xyXG5cclxuICBhc3luYyBmdW5jdGlvbiBhY3RpdmF0ZUFjY291bnQoKSB7XHJcbiAgICBjb25zdCByZXNwb25zZSA9IGF3YWl0IGZldGNoKGAvYXBpL2FjdGl2YXRlLyR7aWR9LyR7dG9rZW59YCwge1xyXG4gICAgICBtZXRob2Q6ICdQVVQnLFxyXG4gICAgfSk7XHJcbiAgICBpZiAocmVzcG9uc2Uub2spIHtcclxuICAgICAgLy8gQWNjb3VudCBhY3RpdmF0ZWQgc3VjY2Vzc2Z1bGx5XHJcbiAgICAgIHJvdXRlci5wdXNoKCcvc3VjY2VzcycpO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgLy8gRXJyb3Igb2NjdXJyZWQgZHVyaW5nIGFjdGl2YXRpb25cclxuICAgICAgY29uc3QgbWVzc2FnZSA9IGF3YWl0IHJlc3BvbnNlLnRleHQoKTtcclxuICAgICAgYWxlcnQoYEZhaWxlZCB0byBhY3RpdmF0ZSBhY2NvdW50OiAke21lc3NhZ2V9YCk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICB1c2VFZmZlY3QoKCkgPT4ge1xyXG4gICAgYWN0aXZhdGVBY2NvdW50KCk7XHJcbiAgfSwgW10pO1xyXG5cclxuICByZXR1cm4gKFxyXG4gICAgPGRpdj5cclxuICAgICAgPGgxPkFjdGl2YXRlIFlvdXIgQWNjb3VudDwvaDE+XHJcbiAgICAgIDxwPkFjdGl2YXRpbmcgeW91ciBhY2NvdW50LCBwbGVhc2Ugd2FpdC4uLjwvcD5cclxuICAgIDwvZGl2PlxyXG4gICk7XHJcbn1cclxuIl0sIm5hbWVzIjpbInVzZVJvdXRlciIsInVzZUVmZmVjdCIsIkFjdGl2YXRlQWNjb3VudCIsInJvdXRlciIsImlkIiwidG9rZW4iLCJxdWVyeSIsImFjdGl2YXRlQWNjb3VudCIsInJlc3BvbnNlIiwiZmV0Y2giLCJtZXRob2QiLCJvayIsInB1c2giLCJtZXNzYWdlIiwidGV4dCIsImFsZXJ0IiwiZGl2IiwiaDEiLCJwIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/pages/ActivationPage.js\n");

/***/ }),

/***/ "next/router":
/*!******************************!*\
  !*** external "next/router" ***!
  \******************************/
/***/ ((module) => {

module.exports = require("next/router");

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "react" ***!
  \************************/
/***/ ((module) => {

module.exports = require("react");

/***/ }),

/***/ "react/jsx-dev-runtime":
/*!****************************************!*\
  !*** external "react/jsx-dev-runtime" ***!
  \****************************************/
/***/ ((module) => {

module.exports = require("react/jsx-dev-runtime");

/***/ })

};
;

// load runtime
var __webpack_require__ = require("../webpack-runtime.js");
__webpack_require__.C(exports);
var __webpack_exec__ = (moduleId) => (__webpack_require__(__webpack_require__.s = moduleId))
var __webpack_exports__ = (__webpack_exec__("./src/pages/ActivationPage.js"));
module.exports = __webpack_exports__;

})();
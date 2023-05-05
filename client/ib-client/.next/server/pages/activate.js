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
exports.id = "pages/activate";
exports.ids = ["pages/activate"];
exports.modules = {

/***/ "./src/styles/activate.module.css":
/*!****************************************!*\
  !*** ./src/styles/activate.module.css ***!
  \****************************************/
/***/ ((module) => {

eval("// Exports\nmodule.exports = {\n\t\"main\": \"activate_main__VTTZs\"\n};\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvc3R5bGVzL2FjdGl2YXRlLm1vZHVsZS5jc3MuanMiLCJtYXBwaW5ncyI6IkFBQUE7QUFDQTtBQUNBO0FBQ0EiLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9pYi1jbGllbnQvLi9zcmMvc3R5bGVzL2FjdGl2YXRlLm1vZHVsZS5jc3M/NDA3NiJdLCJzb3VyY2VzQ29udGVudCI6WyIvLyBFeHBvcnRzXG5tb2R1bGUuZXhwb3J0cyA9IHtcblx0XCJtYWluXCI6IFwiYWN0aXZhdGVfbWFpbl9fVlRUWnNcIlxufTtcbiJdLCJuYW1lcyI6W10sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/styles/activate.module.css\n");

/***/ }),

/***/ "./src/pages/activate.js":
/*!*******************************!*\
  !*** ./src/pages/activate.js ***!
  \*******************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (/* binding */ ActivateAccount)\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"react/jsx-dev-runtime\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! next/router */ \"next/router\");\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(next_router__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react */ \"react\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var _styles_activate_module_css__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../styles/activate.module.css */ \"./src/styles/activate.module.css\");\n/* harmony import */ var _styles_activate_module_css__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_styles_activate_module_css__WEBPACK_IMPORTED_MODULE_3__);\n\n\n\n\nfunction ActivateAccount() {\n    const router = (0,next_router__WEBPACK_IMPORTED_MODULE_1__.useRouter)();\n    const [id, setId] = (0,react__WEBPACK_IMPORTED_MODULE_2__.useState)(null);\n    const [token, setToken] = (0,react__WEBPACK_IMPORTED_MODULE_2__.useState)(null);\n    (0,react__WEBPACK_IMPORTED_MODULE_2__.useEffect)(()=>{\n        setId(router.query.id);\n        setToken(router.query.token);\n    }, [\n        router.query\n    ]);\n    (0,react__WEBPACK_IMPORTED_MODULE_2__.useEffect)(()=>{\n        if (id && token) {\n            activateAccount();\n        }\n    }, [\n        id,\n        token\n    ]);\n    async function activateAccount() {\n        const response = await fetch(`http://localhost:8000/api/user/activate/${id}/${token}`, {\n            method: \"PUT\"\n        });\n        if (response.ok) {\n            // Account activated successfully\n            window.alert(\"bonus nispeasd\");\n            router.push(\"\");\n        } else {\n            // Error occurred during activation\n            const message = await response.text();\n            alert(`Failed to activate account: ${message}`);\n        }\n    }\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n        className: (_styles_activate_module_css__WEBPACK_IMPORTED_MODULE_3___default().main),\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"h1\", {\n                children: \"Activate Your Account\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n                lineNumber: 39,\n                columnNumber: 7\n            }, this),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"p\", {\n                children: \"Activating your account, please wait...\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n                lineNumber: 40,\n                columnNumber: 7\n            }, this)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n        lineNumber: 38,\n        columnNumber: 5\n    }, this);\n}\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvcGFnZXMvYWN0aXZhdGUuanMuanMiLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7OztBQUF3QztBQUNJO0FBQ087QUFHcEMsU0FBU0ksa0JBQWtCO0lBQ3hDLE1BQU1DLFNBQVNMLHNEQUFTQTtJQUN4QixNQUFNLENBQUNNLElBQUlDLE1BQU0sR0FBR0wsK0NBQVFBLENBQUMsSUFBSTtJQUNqQyxNQUFNLENBQUNNLE9BQU9DLFNBQVMsR0FBR1AsK0NBQVFBLENBQUMsSUFBSTtJQUV2Q0QsZ0RBQVNBLENBQUMsSUFBTTtRQUNkTSxNQUFNRixPQUFPSyxLQUFLLENBQUNKLEVBQUU7UUFDckJHLFNBQVNKLE9BQU9LLEtBQUssQ0FBQ0YsS0FBSztJQUM3QixHQUFHO1FBQUNILE9BQU9LLEtBQUs7S0FBQztJQUVqQlQsZ0RBQVNBLENBQUMsSUFBTTtRQUNkLElBQUlLLE1BQU1FLE9BQU87WUFDZkc7UUFDRixDQUFDO0lBQ0gsR0FBRztRQUFDTDtRQUFJRTtLQUFNO0lBRWQsZUFBZUcsa0JBQWtCO1FBQy9CLE1BQU1DLFdBQVcsTUFBTUMsTUFBTSxDQUFDLHdDQUF3QyxFQUFFUCxHQUFHLENBQUMsRUFBRUUsTUFBTSxDQUFDLEVBQUU7WUFDckZNLFFBQVE7UUFDVjtRQUNBLElBQUlGLFNBQVNHLEVBQUUsRUFBRTtZQUNmLGlDQUFpQztZQUNqQ0MsT0FBT0MsS0FBSyxDQUFDO1lBQ2JaLE9BQU9hLElBQUksQ0FBQztRQUNkLE9BQU87WUFDTCxtQ0FBbUM7WUFDbkMsTUFBTUMsVUFBVSxNQUFNUCxTQUFTUSxJQUFJO1lBQ25DSCxNQUFNLENBQUMsNEJBQTRCLEVBQUVFLFFBQVEsQ0FBQztRQUNoRCxDQUFDO0lBQ0g7SUFFQSxxQkFDRSw4REFBQ0U7UUFBSUMsV0FBV25CLHlFQUFXOzswQkFDekIsOERBQUNxQjswQkFBRzs7Ozs7OzBCQUNKLDhEQUFDQzswQkFBRTs7Ozs7Ozs7Ozs7O0FBR1QsQ0FBQyIsInNvdXJjZXMiOlsid2VicGFjazovL2liLWNsaWVudC8uL3NyYy9wYWdlcy9hY3RpdmF0ZS5qcz81Y2Y1Il0sInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IHVzZVJvdXRlciB9IGZyb20gJ25leHQvcm91dGVyJztcclxuaW1wb3J0IHsgdXNlRWZmZWN0LCB1c2VTdGF0ZSB9IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0IHN0eWxlcyBmcm9tICcuLi9zdHlsZXMvYWN0aXZhdGUubW9kdWxlLmNzcyc7XHJcblxyXG5cclxuZXhwb3J0IGRlZmF1bHQgZnVuY3Rpb24gQWN0aXZhdGVBY2NvdW50KCkge1xyXG4gIGNvbnN0IHJvdXRlciA9IHVzZVJvdXRlcigpO1xyXG4gIGNvbnN0IFtpZCwgc2V0SWRdID0gdXNlU3RhdGUobnVsbCk7XHJcbiAgY29uc3QgW3Rva2VuLCBzZXRUb2tlbl0gPSB1c2VTdGF0ZShudWxsKTtcclxuXHJcbiAgdXNlRWZmZWN0KCgpID0+IHtcclxuICAgIHNldElkKHJvdXRlci5xdWVyeS5pZCk7XHJcbiAgICBzZXRUb2tlbihyb3V0ZXIucXVlcnkudG9rZW4pO1xyXG4gIH0sIFtyb3V0ZXIucXVlcnldKTtcclxuXHJcbiAgdXNlRWZmZWN0KCgpID0+IHtcclxuICAgIGlmIChpZCAmJiB0b2tlbikge1xyXG4gICAgICBhY3RpdmF0ZUFjY291bnQoKTtcclxuICAgIH1cclxuICB9LCBbaWQsIHRva2VuXSk7XHJcblxyXG4gIGFzeW5jIGZ1bmN0aW9uIGFjdGl2YXRlQWNjb3VudCgpIHtcclxuICAgIGNvbnN0IHJlc3BvbnNlID0gYXdhaXQgZmV0Y2goYGh0dHA6Ly9sb2NhbGhvc3Q6ODAwMC9hcGkvdXNlci9hY3RpdmF0ZS8ke2lkfS8ke3Rva2VufWAsIHtcclxuICAgICAgbWV0aG9kOiAnUFVUJyxcclxuICAgIH0pO1xyXG4gICAgaWYgKHJlc3BvbnNlLm9rKSB7XHJcbiAgICAgIC8vIEFjY291bnQgYWN0aXZhdGVkIHN1Y2Nlc3NmdWxseVxyXG4gICAgICB3aW5kb3cuYWxlcnQoXCJib251cyBuaXNwZWFzZFwiKVxyXG4gICAgICByb3V0ZXIucHVzaCgnJyk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAvLyBFcnJvciBvY2N1cnJlZCBkdXJpbmcgYWN0aXZhdGlvblxyXG4gICAgICBjb25zdCBtZXNzYWdlID0gYXdhaXQgcmVzcG9uc2UudGV4dCgpO1xyXG4gICAgICBhbGVydChgRmFpbGVkIHRvIGFjdGl2YXRlIGFjY291bnQ6ICR7bWVzc2FnZX1gKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIHJldHVybiAoXHJcbiAgICA8ZGl2IGNsYXNzTmFtZT17c3R5bGVzLm1haW59PlxyXG4gICAgICA8aDE+QWN0aXZhdGUgWW91ciBBY2NvdW50PC9oMT5cclxuICAgICAgPHA+QWN0aXZhdGluZyB5b3VyIGFjY291bnQsIHBsZWFzZSB3YWl0Li4uPC9wPlxyXG4gICAgPC9kaXY+XHJcbiAgKTtcclxufVxyXG4iXSwibmFtZXMiOlsidXNlUm91dGVyIiwidXNlRWZmZWN0IiwidXNlU3RhdGUiLCJzdHlsZXMiLCJBY3RpdmF0ZUFjY291bnQiLCJyb3V0ZXIiLCJpZCIsInNldElkIiwidG9rZW4iLCJzZXRUb2tlbiIsInF1ZXJ5IiwiYWN0aXZhdGVBY2NvdW50IiwicmVzcG9uc2UiLCJmZXRjaCIsIm1ldGhvZCIsIm9rIiwid2luZG93IiwiYWxlcnQiLCJwdXNoIiwibWVzc2FnZSIsInRleHQiLCJkaXYiLCJjbGFzc05hbWUiLCJtYWluIiwiaDEiLCJwIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/pages/activate.js\n");

/***/ }),

/***/ "next/router":
/*!******************************!*\
  !*** external "next/router" ***!
  \******************************/
/***/ ((module) => {

"use strict";
module.exports = require("next/router");

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "react" ***!
  \************************/
/***/ ((module) => {

"use strict";
module.exports = require("react");

/***/ }),

/***/ "react/jsx-dev-runtime":
/*!****************************************!*\
  !*** external "react/jsx-dev-runtime" ***!
  \****************************************/
/***/ ((module) => {

"use strict";
module.exports = require("react/jsx-dev-runtime");

/***/ })

};
;

// load runtime
var __webpack_require__ = require("../webpack-runtime.js");
__webpack_require__.C(exports);
var __webpack_exec__ = (moduleId) => (__webpack_require__(__webpack_require__.s = moduleId))
var __webpack_exports__ = (__webpack_exec__("./src/pages/activate.js"));
module.exports = __webpack_exports__;

})();
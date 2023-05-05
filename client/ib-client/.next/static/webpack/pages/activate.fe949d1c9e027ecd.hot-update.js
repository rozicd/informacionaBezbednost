"use strict";
/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
self["webpackHotUpdate_N_E"]("pages/activate",{

/***/ "./src/pages/activate.js":
/*!*******************************!*\
  !*** ./src/pages/activate.js ***!
  \*******************************/
/***/ (function(module, __webpack_exports__, __webpack_require__) {

eval(__webpack_require__.ts("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": function() { return /* binding */ ActivateAccount; }\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"./node_modules/react/jsx-dev-runtime.js\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! next/router */ \"./node_modules/next/router.js\");\n/* harmony import */ var next_router__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(next_router__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react */ \"./node_modules/react/index.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_2__);\n\nvar _s = $RefreshSig$();\n\n\nfunction ActivateAccount() {\n    _s();\n    const router = (0,next_router__WEBPACK_IMPORTED_MODULE_1__.useRouter)();\n    const { id , token  } = router.query;\n    async function activateAccount() {\n        const response = await fetch(\"http://localhost:8000/api/activate/\".concat(id, \"/\").concat(token), {\n            method: \"PUT\"\n        });\n        if (response.ok) {\n            // Account activated successfully\n            router.push(\"/success\");\n        } else {\n            // Error occurred during activation\n            const message = await response.text();\n            alert(\"Failed to activate account: \".concat(message));\n        }\n    }\n    (0,react__WEBPACK_IMPORTED_MODULE_2__.useEffect)(()=>{\n        activateAccount();\n    }, []);\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"h1\", {\n                children: \"Activate Your Account\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n                lineNumber: 28,\n                columnNumber: 7\n            }, this),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"p\", {\n                children: \"Activating your account, please wait...\"\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n                lineNumber: 29,\n                columnNumber: 7\n            }, this)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Dusan\\\\Desktop\\\\VI_semestar\\\\IB\\\\IBProjekat\\\\client\\\\ib-client\\\\src\\\\pages\\\\activate.js\",\n        lineNumber: 27,\n        columnNumber: 5\n    }, this);\n}\n_s(ActivateAccount, \"vQduR7x+OPXj6PSmJyFnf+hU7bg=\", false, function() {\n    return [\n        next_router__WEBPACK_IMPORTED_MODULE_1__.useRouter\n    ];\n});\n_c = ActivateAccount;\nvar _c;\n$RefreshReg$(_c, \"ActivateAccount\");\n\n\n;\n    // Wrapped in an IIFE to avoid polluting the global scope\n    ;\n    (function () {\n        var _a, _b;\n        // Legacy CSS implementations will `eval` browser code in a Node.js context\n        // to extract CSS. For backwards compatibility, we need to check we're in a\n        // browser context before continuing.\n        if (typeof self !== 'undefined' &&\n            // AMP / No-JS mode does not inject these helpers:\n            '$RefreshHelpers$' in self) {\n            // @ts-ignore __webpack_module__ is global\n            var currentExports = module.exports;\n            // @ts-ignore __webpack_module__ is global\n            var prevExports = (_b = (_a = module.hot.data) === null || _a === void 0 ? void 0 : _a.prevExports) !== null && _b !== void 0 ? _b : null;\n            // This cannot happen in MainTemplate because the exports mismatch between\n            // templating and execution.\n            self.$RefreshHelpers$.registerExportsForReactRefresh(currentExports, module.id);\n            // A module can be accepted automatically based on its exports, e.g. when\n            // it is a Refresh Boundary.\n            if (self.$RefreshHelpers$.isReactRefreshBoundary(currentExports)) {\n                // Save the previous exports on update so we can compare the boundary\n                // signatures.\n                module.hot.dispose(function (data) {\n                    data.prevExports = currentExports;\n                });\n                // Unconditionally accept an update to this module, we'll check if it's\n                // still a Refresh Boundary later.\n                // @ts-ignore importMeta is replaced in the loader\n                module.hot.accept();\n                // This field is set when the previous version of this module was a\n                // Refresh Boundary, letting us know we need to check for invalidation or\n                // enqueue an update.\n                if (prevExports !== null) {\n                    // A boundary can become ineligible if its exports are incompatible\n                    // with the previous exports.\n                    //\n                    // For example, if you add/remove/change exports, we'll want to\n                    // re-execute the importing modules, and force those components to\n                    // re-render. Similarly, if you convert a class component to a\n                    // function, we want to invalidate the boundary.\n                    if (self.$RefreshHelpers$.shouldInvalidateReactRefreshBoundary(prevExports, currentExports)) {\n                        module.hot.invalidate();\n                    }\n                    else {\n                        self.$RefreshHelpers$.scheduleUpdate();\n                    }\n                }\n            }\n            else {\n                // Since we just executed the code for the module, it's possible that the\n                // new exports made it ineligible for being a boundary.\n                // We only care about the case when we were _previously_ a boundary,\n                // because we already accepted this update (accidental side effect).\n                var isNoLongerABoundary = prevExports !== null;\n                if (isNoLongerABoundary) {\n                    module.hot.invalidate();\n                }\n            }\n        }\n    })();\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvcGFnZXMvYWN0aXZhdGUuanMuanMiLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7O0FBQXdDO0FBQ047QUFFbkIsU0FBU0Usa0JBQWtCOztJQUN4QyxNQUFNQyxTQUFTSCxzREFBU0E7SUFDeEIsTUFBTSxFQUFFSSxHQUFFLEVBQUVDLE1BQUssRUFBRSxHQUFHRixPQUFPRyxLQUFLO0lBRWxDLGVBQWVDLGtCQUFrQjtRQUMvQixNQUFNQyxXQUFXLE1BQU1DLE1BQU0sc0NBQTRDSixPQUFORCxJQUFHLEtBQVMsT0FBTkMsUUFBUztZQUNoRkssUUFBUTtRQUNWO1FBQ0EsSUFBSUYsU0FBU0csRUFBRSxFQUFFO1lBQ2YsaUNBQWlDO1lBQ2pDUixPQUFPUyxJQUFJLENBQUM7UUFDZCxPQUFPO1lBQ0wsbUNBQW1DO1lBQ25DLE1BQU1DLFVBQVUsTUFBTUwsU0FBU00sSUFBSTtZQUNuQ0MsTUFBTSwrQkFBdUMsT0FBUkY7UUFDdkMsQ0FBQztJQUNIO0lBRUFaLGdEQUFTQSxDQUFDLElBQU07UUFDZE07SUFDRixHQUFHLEVBQUU7SUFFTCxxQkFDRSw4REFBQ1M7OzBCQUNDLDhEQUFDQzswQkFBRzs7Ozs7OzBCQUNKLDhEQUFDQzswQkFBRTs7Ozs7Ozs7Ozs7O0FBR1QsQ0FBQztHQTVCdUJoQjs7UUFDUEYsa0RBQVNBOzs7S0FERkUiLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9fTl9FLy4vc3JjL3BhZ2VzL2FjdGl2YXRlLmpzPzVjZjUiXSwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgdXNlUm91dGVyIH0gZnJvbSAnbmV4dC9yb3V0ZXInO1xyXG5pbXBvcnQgeyB1c2VFZmZlY3QgfSBmcm9tICdyZWFjdCc7XHJcblxyXG5leHBvcnQgZGVmYXVsdCBmdW5jdGlvbiBBY3RpdmF0ZUFjY291bnQoKSB7XHJcbiAgY29uc3Qgcm91dGVyID0gdXNlUm91dGVyKCk7XHJcbiAgY29uc3QgeyBpZCwgdG9rZW4gfSA9IHJvdXRlci5xdWVyeTtcclxuXHJcbiAgYXN5bmMgZnVuY3Rpb24gYWN0aXZhdGVBY2NvdW50KCkge1xyXG4gICAgY29uc3QgcmVzcG9uc2UgPSBhd2FpdCBmZXRjaChgaHR0cDovL2xvY2FsaG9zdDo4MDAwL2FwaS9hY3RpdmF0ZS8ke2lkfS8ke3Rva2VufWAsIHtcclxuICAgICAgbWV0aG9kOiAnUFVUJyxcclxuICAgIH0pO1xyXG4gICAgaWYgKHJlc3BvbnNlLm9rKSB7XHJcbiAgICAgIC8vIEFjY291bnQgYWN0aXZhdGVkIHN1Y2Nlc3NmdWxseVxyXG4gICAgICByb3V0ZXIucHVzaCgnL3N1Y2Nlc3MnKTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIC8vIEVycm9yIG9jY3VycmVkIGR1cmluZyBhY3RpdmF0aW9uXHJcbiAgICAgIGNvbnN0IG1lc3NhZ2UgPSBhd2FpdCByZXNwb25zZS50ZXh0KCk7XHJcbiAgICAgIGFsZXJ0KGBGYWlsZWQgdG8gYWN0aXZhdGUgYWNjb3VudDogJHttZXNzYWdlfWApO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgdXNlRWZmZWN0KCgpID0+IHtcclxuICAgIGFjdGl2YXRlQWNjb3VudCgpO1xyXG4gIH0sIFtdKTtcclxuXHJcbiAgcmV0dXJuIChcclxuICAgIDxkaXY+XHJcbiAgICAgIDxoMT5BY3RpdmF0ZSBZb3VyIEFjY291bnQ8L2gxPlxyXG4gICAgICA8cD5BY3RpdmF0aW5nIHlvdXIgYWNjb3VudCwgcGxlYXNlIHdhaXQuLi48L3A+XHJcbiAgICA8L2Rpdj5cclxuICApO1xyXG59XHJcbiJdLCJuYW1lcyI6WyJ1c2VSb3V0ZXIiLCJ1c2VFZmZlY3QiLCJBY3RpdmF0ZUFjY291bnQiLCJyb3V0ZXIiLCJpZCIsInRva2VuIiwicXVlcnkiLCJhY3RpdmF0ZUFjY291bnQiLCJyZXNwb25zZSIsImZldGNoIiwibWV0aG9kIiwib2siLCJwdXNoIiwibWVzc2FnZSIsInRleHQiLCJhbGVydCIsImRpdiIsImgxIiwicCJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./src/pages/activate.js\n"));

/***/ })

});
let config = {
    mode: "fixed_servers",
    rules: {
        singleProxy: {
            scheme: "http",
            host: "{IP}",
            port: parseInt({PORT})
        },
        bypassList: [{BYPASS_LIST}]
    }
};

chrome.proxy.settings.set({
    value: config,
    scope: "regular"
}, function () {});

function callbackFn() {
    return {
        authCredentials: {
            username: "{CREDENTIALS_USER}",
            password: "{CREDENTIALS_PASS}"
        }
    };
}

chrome.webRequest.onAuthRequired.addListener(
    callbackFn, {
        urls: ["<all_urls>"]
    },
    ['blocking']
);
var authContext = null;
var user = null;

window.blazorInterop = {
    void: function(){
        return null;
    },
    isLoggedIn: function() {
        // TODO: also check that the context has a valid user
        return authContext != null;
    },
    getUserName: function () {
        if (user === null) {
            console.log(`'user' is null in getUserName()`);
            return Blazor.platform.toDotNetString('');
        }
        var userName = user.userName;
        // console.log(`'userName' is ${userName} in getUserName()`);
        return userName;
    },  
    getTenantId: function () {
        if (user === null) {
            console.log(`'user' is null in getTenantId()`);
            return null;
        }

        return user.profile.tid;
    },
    getHostname: function () {

        // console.log(window.location);
        var hostname = `${window.location.hostname}`;

        // console.log(`'hostname' is ${hostname} in getHostname()`);

        return hostname;
    },
    login: function(redirectPath) {
        
        var redirectUri = window.location.origin + redirectPath;
        
        // in place of when page loads
        window.config = {
            instance: 'https://login.microsoftonline.com/',
            tenant: 'common',
            clientId: '04ae7823-535e-4341-8cee-577a1cfca687',
            postLogoutRedirectUri: window.location.origin,
            redirectUri: redirectUri,
            cacheLocation: 'localStorage' // enable this for IE, as sessionStorage does not work for localhost.
        };
        // console.log(`Callback uri ${config.postLogoutRedirectUri}`);
        // this is set as a local variable due. to strict mode?
        authContext = new AuthenticationContext(config);
        var isCallback = authContext.isCallback(window.location.hash);
        console.log("isCallback: " + isCallback);
        if(isCallback)
        {
            // this may not redirect if already logged in
            console.info("handle the window callback and redirect to " +  window.config.redirectUri);
            authContext.handleWindowCallback();
        }
        // otherwise get the cached user
        
        //$errorMessage.html(authContext.getLoginError());

        if (isCallback && !authContext.getLoginError()) {
            console.info("setting window.location to " + redirectUri);
            window.location = redirectUri; //authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);
            // does the rest of the code run?
        }

        // Check Login Status, Update UI
        user = authContext.getCachedUser();
        console.info(user);
        // console.log(user);
        if (!user) {
            // this will redirect
            console.info("user not logged in, running authContext.login()");
            authContext.login();
        }
    },
    executeWithToken: function (action) {
        // console.log(`executeWithToken() for clientId ${clientId}, authContext.config.clientId = ${authContext.config.clientId}`);
        try {
            authContext.acquireToken(authContext.config.clientId, function (error, token) {
                let tokenString = Blazor.platform.toDotNetString(token);
                // console.log(`tokenstring: ${tokenString}`);
                const assemblyName = 'blazor.wa.aadauth.sample';
                const namespace = 'blazor.wa.aadauth.sample';
                const typeName = 'AdalHelper';
                const methodName = 'RunAction';
    
                const runActionMethod = Blazor.platform.findMethod(
                    assemblyName,
                    namespace,
                    typeName,
                    methodName
                );
    
                Blazor.platform.callMethod(runActionMethod, null, [
                    action, tokenString
                ]);
            });
        } catch (error) {
            // console.error(`executeWithToken() failed for clientId ${clientId}`);
            return false;
        }
        return true;
    }
};

(function () {
    // we only want this to run if we are not on the main page (root)
    var isRoot = window.location.pathname == null || window.location.pathname == '/';
    // console.log(window.location.protocol);
    // console.log(window.location.host);
    // console.log(window.location.pathname);
    // console.log(window.location.origin);
    console.info("path is " + window.location.pathname);
    if(isRoot)
    {
        // use the login button!
        console.info("On root page, not running automatic login script");
        return;
    }

    // redirect to same path as we already are
    // console.info("redirect uri is " + window.location.origin + window.location.pathname);
    // something messed up with scope?
    window.blazorInterop.login(window.location.pathname);
}());
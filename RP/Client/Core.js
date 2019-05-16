let login = null;
let loginCamera = null;

let camPos = new Vector3(200.0,-800.0,350.0);
let camTargetPos = new Vector3(200.0,-800.0,2.0);

let time = 0.0;

API.onResourceStart.connect(function() {
	let screenResolution = API.getScreenResolution();
	login = API.createCefBrowser(400, 400, true);
	API.waitUntilCefBrowserInit(login);
	API.setCefBrowserPosition(login, screenResolution.Width/2-200, screenResolution.Height/2-200);
	API.loadPageCefBrowser(login, "Client/UI/login.html");
	API.setCefBrowserHeadless(login, false);

	API.showCursor(true);
	API.setChatVisible(false);
	API.setCanOpenChat(false);
	API.setHudVisible(false);

	loginCamera = API.createCamera(camPos, new Vector3());
	API.pointCameraAtPosition(loginCamera, camTargetPos);
	API.setCameraPosition(loginCamera, camPos);
	API.setActiveCamera(loginCamera);
});

var start = new Date().getTime();
function getTickCount() {
    var now = new Date().getTime();

    return (now - start) / 1000.0;
}

const DEGREE_TO_RADIAN = 0.0174532925;

var lastFrameTime = 0;
API.onUpdate.connect(function () {
    var now = getTickCount();
    var delta = now - lastFrameTime;
    lastFrameTime = now;

	if (loginCamera) {
		camPos.X = camTargetPos.X + Math.sin(time) * 1500.0;
		camPos.Y = camTargetPos.Y + Math.cos(time) * 1500.0;
		API.setCameraPosition(loginCamera, camPos);

		time += DEGREE_TO_RADIAN * 2.0 * delta;
	}
});

API.onServerEventTrigger.connect(function(name, args)
{
	if (name == "loginResult") {
		let success = args[0];
		if (! success) {
			login.call("onLoginError", args[1]+"");
		}
		else {
			API.destroyCefBrowser(login);
		}
	}
	else if (name == "playerSpawn") {
	    API.showCursor(false);
	    API.setChatVisible(true);
	    API.setCanOpenChat(true);
	    API.setHudVisible(true);

	    // todo destroy camera
	    API.setActiveCamera(null);
	    loginCamera = null;
	}
});

function DoLogin(username, password)
{
	API.triggerServerEvent("doLogin", username, password);
}


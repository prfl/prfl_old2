// Set the page title
function SetTitle(title) {
  document.title = title;
}


// Save visitor state
window.setVisitor = (obj) => {
  //let device = {'deviceId': obj.deviceId, 'createdOn': obj.createdOn, 'page': obj.page };
  localStorage.setItem("prfl-visitorId", obj.visitorId);
  localStorage.setItem("prfl-visitedOn", obj.visitedOn);
  localStorage.setItem("prfl-page", obj.page);
};


let isHidden = true;

function initFreshChat(isHidden) {
  window.fcWidget.init({
    "config": {
      "headerProperty": {
      "hideChatButton": isHidden
      }
    },
    token: "533e17d6-146e-4720-8c6e-bcd1b82c0590",
    host: "https://wchat.freshchat.com"
  }); 
}

function initialize(i,t){
  var e;i.getElementById(t)?initFreshChat(isHidden):((e=i.createElement("script")).id=t,
      e.async=!0,e.src="https://wchat.freshchat.com/js/widget.js",
      e.onload=initFreshChat(isHidden),i.head.appendChild(e))
    }
    function initiateCall(){
      initialize(document,"freshchat-js-sdk")
    }
    window.addEventListener?window.addEventListener("load",initiateCall,!1):window.attachEvent("load",initiateCall,!1);



    function CheckArea(id) {
      let element = document.getElementById(id);
      let classValue = element.getAttribute('aria-expanded');
      console.log(classValue);
      if(classValue.includes("true")) {
        return true;
      }
      else {
        return false;
      }
        
    }
    
window.exampleJsFunctions = {
  uploadImage: function () {
    let imageHandle = "";
    const apikey = 'AV4oJQB1wSR6myENuSOlkz';
    const client = filestack.init(apikey);
    
    const options = {
      uploadInBackground: false,
      onFileSelected: file => {
        // If you throw any error in this function it will reject the file selection.
        // The error message will be displayed to the user as an alert.
        if (file.size > 1000 * 1000) {
            throw new Error('File too big, select something smaller than 1MB');
        }
      },
      onFileUploadFinished: (response) => {
        // after file upload, make request with data to your application
          imageHandle = response.handle;
          console.log(imageHandle);
          DotNet.invokeMethodAsync('Profile.Client', 'UploadImage', imageHandle)
        }
      }
      const picker = client.picker(options);
      picker.open();
    }
    
  }
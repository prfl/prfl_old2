


function SetTitle(title) {
  document.title = title;
}



window.setVisitor = (obj) => {
  //let device = {'deviceId': obj.deviceId, 'createdOn': obj.createdOn, 'page': obj.page };
  localStorage.setItem("prfl-visitorId", obj.visitorId);
  localStorage.setItem("prfl-visitedOn", obj.visitedOn);
  localStorage.setItem("prfl-page", obj.page);
};


// window.addTooltips = () => {
//   var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
//   var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
//   return new bootstrap.Tooltip(tooltipTriggerEl)
//   })
// }




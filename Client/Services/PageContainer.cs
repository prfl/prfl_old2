using System;

namespace Profile.Client.Services {
    public class PageContainer
    {
        public string LinkType { get; set; }
        public string PreviousPage { get; set; }
        public event Action OnChange;
    
        public void SetProperty(string value)
        {
            LinkType = value;
            NotifyStateChanged();
        }
        public void SetPreviousPage(string url) {
            PreviousPage = url;
        }

        public string GetProperty() {
            return LinkType;
        }
        
        public string GetPreviousPage() {
            return PreviousPage;
        }
    
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

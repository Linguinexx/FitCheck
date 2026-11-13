using UnityEngine;
namespace FitCheck
{
    public partial class FirstCustomer
    {
        Texture2D fitFeedStars;
        void DrawFeedBackdrop(){
            Fill(new Rect(0,0,1440,900),Color.black);
            if(fitFeedStars==null)fitFeedStars=Resources.Load<Texture2D>("UI/FitFeedStars");
            if(fitFeedStars!=null){var color=GUI.color;GUI.color=Color.white;GUI.DrawTexture(new Rect(0,0,1440,900),fitFeedStars,ScaleMode.ScaleAndCrop);GUI.color=color;}
            // Keep the navigation and labels quiet; the pattern shows in the
            // gutters around the opaque cards without changing their layout.
            Fill(new Rect(0,0,424,900),new Color(FeedBackground.r,FeedBackground.g,FeedBackground.b,.94f));
        }
    }
}

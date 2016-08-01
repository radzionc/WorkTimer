using System;

namespace XamarinWorkTimer
{
    public interface IReminderService
    {
        void Remind(int seconds, string title, string message);
        void CancelRemind();
    }
}
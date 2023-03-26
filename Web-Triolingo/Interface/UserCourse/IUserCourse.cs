using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.UserCourse
{
    public interface IUserCourse
    {
        List<int> getUserIdInCourse(int courseId);
        void updateMentorAdd(List<User> mentors, List<bool> isMentor,int courseId);
    }
}

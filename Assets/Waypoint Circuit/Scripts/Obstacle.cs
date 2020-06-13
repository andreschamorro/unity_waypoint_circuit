using UnityEngine;

namespace Waypoint
{
    public class Obstacle : MonoBehaviour
    {
        #region FIELDS

        [SerializeField]
        private bool _pathIsBusy = false;

        public bool PathIsBusy
        {
            get => _pathIsBusy;
            set => _pathIsBusy = value;
        }

        #endregion
    }
}
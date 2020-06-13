using UnityEngine;

namespace Waypoint
{
    public struct RoutePoint
    {
        private Vector3 _position;
        private Vector3 _direction;

        public RoutePoint(Vector3 position, Vector3 direction)
        {
            this._position = position;
            this._direction = direction;
        }
    }
}
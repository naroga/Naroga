using UnityEngine;
using System.Collections;

namespace Naroga.InputController
{

    public class InputController : MonoBehaviour, IInputController
    {

        protected event SChangeHorizontalAxis onChangeHorizontalAxis;
        protected event SChangeVerticalAxis onChangeVerticalAxis;

        protected float horizontalAxis = 0f;
        protected float verticalAxis = 0f;

        public void Update()
        {

            float newHorizontalAxis = Input.GetAxis("Horizontal");
            float newVerticalAxis = Input.GetAxis("Vertical");

            if (newHorizontalAxis != horizontalAxis)
            {
                horizontalAxis = newHorizontalAxis;
                if (onChangeHorizontalAxis != null)
                {
                    onChangeHorizontalAxis(horizontalAxis);
                }
            }

            if (newVerticalAxis != verticalAxis)
            {
                verticalAxis = newVerticalAxis;
                if (onChangeVerticalAxis != null)
                {
                    onChangeVerticalAxis(verticalAxis);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void addHorizontalAxisChangeListener(SChangeHorizontalAxis callback)
        {
            Debug.Log("Entrei!");
            this.onChangeHorizontalAxis += callback;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void addVerticalAxisChangeListener(SChangeVerticalAxis callback)
        {
            this.onChangeVerticalAxis += callback;
        }
    }

}
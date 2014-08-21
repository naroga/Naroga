using UnityEngine;
using System.Collections;
using Naroga.InputController;

namespace Naroga.CharacterController.ThirdPerson
{

    public class ThirdPerson : MonoBehaviour
    {

        //IoC initiation
        protected IInputController _inputController;

        public void setInputController(IInputController controller)
        {
            _inputController = controller;
        }
        
        // Use this for initialization
        void Start()
        {
            _inputController.addHorizontalAxisChangeListener(updateHorizontalAxis);
        }

        public void updateHorizontalAxis(float newValue)
        {

        }


    }

}
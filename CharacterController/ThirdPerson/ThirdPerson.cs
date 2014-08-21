using UnityEngine;
using System.Collections;
using Naroga.InputController;

namespace Naroga.CharacterController.ThirdPerson
{

    public class ThirdPerson : MonoBehaviour
    {

        //IoC initiation
        protected IInputController inputController;
        protected UnityEngine.CharacterController characterController;
        
        public float speed = 1f;

        public float horizontalAxis = 0;
        public float verticalAxis = 0;

        public void setInputController(IInputController controller)
        {
            inputController = controller;
        }

        public void setCharacterController(UnityEngine.CharacterController controller)
        {
            characterController = controller;
        }

        protected void Update()
        {
            
            Transform characterTransform = characterController.transform;

            characterController.SimpleMove((characterTransform.right * horizontalAxis + characterTransform.forward * verticalAxis).normalized * speed);

        }        
        
        // Use this for initialization
        void Start()
        {
            inputController.addHorizontalAxisChangeListener(updateHorizontalAxis);
            inputController.addVerticalAxisChangeListener(updateVerticalAxis);
        }

        public void updateHorizontalAxis(float newValue)
        {
            horizontalAxis = newValue;
        }

        public void updateVerticalAxis(float newValue)
        {
            verticalAxis = newValue;
        }


    }

}
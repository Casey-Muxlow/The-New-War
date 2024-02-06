using System.Threading.Tasks;
using UnityEngine;

namespace DIG.Tweening
{
    public static class LerpHelper 
    {
        /// <summary>
        /// Lerps transform to a given possition by the duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endPosition"></param>
        /// <param name="duration"></param>
        public static async void LerpPosition(this Transform target, Vector3 endPosition, float duration)
        {
            float elapsedTime = 0;
            Vector3 startPossition = target.localPosition;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                target.localPosition = Vector3.Lerp(startPossition, endPosition, elapsedTime / duration);

                await Task.Yield();
            }

            //Makes sure possition is correct after animation
            target.localPosition = endPosition;

            await Task.Yield();
        }

        /// <summary>
        /// Lerps transform on X axis to a given position possition by the duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endPosition"></param>
        /// <param name="duration"></param>
        public static async void LerpPositionX(this Transform target, float endPosition, float duration)
        {
            float elapsedTime = 0;
            float startPossitionX = target.localPosition.x;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                target.localPosition = new Vector3(Mathf.Lerp(startPossitionX, endPosition, elapsedTime / duration),
                    target.localPosition.y);

                await Task.Yield();
            }

            //Makes sure possition is correct after animation
            target.localPosition = new Vector3(endPosition,
                    target.localPosition.y); 

            await Task.Yield();
        }

        /// <summary>
        /// Lerps transform on Y axis to a given position possition by the duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="endPosition"></param>
        /// <param name="duration"></param>
        public static async void LerpPositionY(this Transform target, float endPosition, float duration)
        {
            float elapsedTime = 0;
            float startPossitionY = target.localPosition.y;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                target.localPosition = new Vector3(target.localPosition.x,
                Mathf.Lerp(startPossitionY, endPosition, elapsedTime / duration));

                await Task.Yield();
            }

            //Makes sure possition is correct after animation
            target.localPosition = new Vector3(target.localPosition.x, endPosition);

            await Task.Yield();
        }
    }

}

// <copyright file="PointcloudVisualizer.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.Common
{
	using GoogleARCore;
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine.UI;

	/// <summary>
	/// Visualize the point cloud.
	/// </summary>
	public class PointcloudVisualizer : MonoBehaviour
	{
		private const int k_MaxPointCount = 61440;

        public AudioSource source;
        public AudioClip leftclip;
        public AudioClip rightclip;
        public AudioClip aheadclip;
        public AudioClip stopclip;

        private Mesh m_Mesh;

		public Camera m_Camera;

		public Text leftT;
		public Text centerT;
		public Text rightT;

        private long timeSinceLast;
        private string lastAdvice;

		//public int distance;

		List<float> sectionLeft = new List<float> ();
		List<float> sectionRight = new List<float> ();
		List<float> sectionCenter = new List<float> ();

		private Vector3[] m_Points = new Vector3[k_MaxPointCount];

		/// <summary>
		/// Unity start.
		/// </summary>
		public void Start ()
		{
            timeSinceLast = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lastAdvice = "ahead";
			m_Mesh = GetComponent<MeshFilter> ().mesh;
			m_Mesh.Clear ();
            
        }

		/// <summary>
		/// Unity update.
		/// </summary>
		public void Update ()
		{
			// Fill in the data to draw the point cloud.
			if (Frame.PointCloud.IsUpdatedThisFrame) {
				// Copy the point cloud points for mesh verticies.
				for (int i = 0; i < Frame.PointCloud.PointCount; i++) {
					m_Points [i] = Frame.PointCloud.GetPointAsStruct (i);
					//Debug.Log (Frame.PointCloud.GetPointAsStruct (i).Position.x + " : " + Frame.PointCloud.GetPointAsStruct (i).Position.y + " : " + Frame.PointCloud.GetPointAsStruct (i).Position.z);
					//Debug.Log (m_Camera.pixelHeight + " " + m_Camera.pixelWidth);
					Vector3 point2 = m_Camera.WorldToScreenPoint (m_Points [i]);
					averageSection (point2);
				}

				applyAverage ();

				// Update the mesh indicies array.
				int[] indices = new int[Frame.PointCloud.PointCount];
				for (int i = 0; i < Frame.PointCloud.PointCount; i++) {
					indices [i] = i;
				}

				m_Mesh.Clear ();
				m_Mesh.vertices = m_Points;
				m_Mesh.SetIndices (indices, MeshTopology.Points, 0);
			}
		}

		private void averageSection (Vector3 point)
		{
			int leftlim = 287;
			int rightlim = 861;

			if (point.x <= leftlim) {
				sectionLeft.Add (point.z);
			} else if (leftlim < point.x && point.x < rightlim) {
				sectionCenter.Add (point.z);
			} else {
				sectionRight.Add (point.z);
			}
		}

		private void applyAverage ()
		{
			double left = sectionLeft.Count > 0 ? sectionLeft.Average () : 0.0;
			double center = sectionCenter.Count > 0 ? sectionCenter.Average () : 0.0;
			double right = sectionRight.Count > 0 ? sectionRight.Average () : 0.0;

			leftT.text = left.ToString();
			centerT.text = center.ToString();
			rightT.text = right.ToString();

            playdir(left, center, right);

			//Debug.Log ("left: " + left + " Center: " + center + " Right: " + right);

			sectionLeft.Clear ();
			sectionCenter.Clear ();
			sectionRight.Clear ();
		}

        private void playdir(double left, double center, double right)
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now - timeSinceLast > 5000)
            {
                bool pass = false;
                source = GetComponent<AudioSource>();
                double[] maxlist = new double[] { left, right, center };
                double max = maxlist.Max();

                if (max < 5)
                {
                    source.clip = stopclip;
                    if (lastAdvice.Equals("stop"))
                        pass = true;
                    lastAdvice = "stop";
                }
                else if (max == left)
                {
                    source.clip = leftclip;
                    if (lastAdvice.Equals("left"))
                        pass = true;
                    lastAdvice = "left";
                }
                else if (max == right)
                {
                    source.clip = rightclip;
                    if (lastAdvice.Equals("right"))
                        pass = true;
                    lastAdvice = "right";
                }
                else
                {
                    source.clip = aheadclip;
                    if (lastAdvice.Equals("ahead"))
                        pass = true;
                    lastAdvice = "ahead";
                }

                if (true)
                {
                    source.Play();
                    timeSinceLast = now;
                }

                
            }

        }
	}
}
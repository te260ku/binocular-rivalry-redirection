///////////////////////////////////////////////////////////////////
///
///BoneViewer.cs ボーンビューア
/// version1.02 2020/3/5
///
///使い方：SkinnedMeshRendererの存在するGameObjectにこのスクリプトをAddCompornentします
///        ViewのGizmos → 3DアイコンがONになっていると各ギズモが見えない事がよくあるので、OFFにする事をおすすめします
///
///Copyright（c）2020 gamebox
///Released under the MIT license
///
/// 説明ページ:
/// http://www.unitygamebox.com/entry/2020/03/03/%E3%80%90unity%E3%80%91unity%E4%B8%8A%E3%81%A7%E3%83%9C%E3%83%BC%E3%83%B3%E3%82%92%E5%8F%AF%E8%A6%96%E5%8C%96%E3%81%99%E3%82%8B%E3%83%84%E3%83%BC%E3%83%AB%E3%80%90%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97
///////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BoneViewer
{

	[ExecuteInEditMode]
	public class BoneViewer : MonoBehaviour
	{
		/// <summary> メッシュ </summary>
		private SkinnedMeshRenderer mMesh;

		/// <summary> 骨のTransform </summary>
		private Transform[] mBones;

		/// <summary> 骨の座標 </summary>
		private List<Vector3> mBoneVector;

		/// <summary> 骨の名前 </summary>
		private List<string> mBoneName;

#if UNITY_EDITOR
		/// <summary> ボーン名表示用のスタイル </summary>
		private GUIStyle mGuiStyle = new GUIStyle();
#endif

		/// <summary> 骨を球で表示するか </summary>
		[Header("【骨を球で表示するか】")]
		public bool ShowBoneMarker = true;

		/// <summary> 骨表示球の大きさ </summary>
		[Header("【骨表示球の大きさ】")]
		[Range(0, 2)]
		public float BoneMarkerSize = 0.1f;

		/// <summary> 骨を表示する球の色 </summary>
		[Header("【骨を表示する球の色】")]
		public Color mBonecolor = Color.blue;

		/// <summary> 骨を結ぶ線の表示 </summary>
		[Header("【骨を結ぶ線の表示】")]
		public bool ShowLines = true;

		/// <summary> 骨を接続する線の色 </summary>
		[Header("【骨を接続する線の色】")]
		public Color mLinecolor = Color.red;

		/// <summary> 骨の名前の表示 </summary>
		[Header("【骨の名前の表示】")]
		public bool ShowBoneName = false;

		/// <summary> 骨の名前表示の色 </summary>
		[Header("【骨の名前表示の色】")]
		public Color mBoneNameColor = Color.black;

		void Awake()
		{
			mMesh = gameObject.GetComponent<SkinnedMeshRenderer>();
			if (mMesh == null) { return; }
			mBones = mMesh.bones;
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (mMesh == null) { return; }

			mBoneVector = new List<Vector3>();
			mBoneName = new List<string>();

			//ボーンの位置と名前を取得
			for (int i = 0; i < mBones.Length; i++)
			{
				mBoneVector.Add( mBones[i].position );
				mBoneName.Add( mBones[i].name );
			}

			//骨を繋ぐラインを表示
			Gizmos.color = mLinecolor;
			if (ShowLines)
			{
				int count = mBones.Length;
				for (int i = 0; i < count; i++)
				{
					Gizmos.DrawLine( mBones[i].parent.position, mBones[i].position );
				}
			}

			//骨の球と名前を表示
			Gizmos.color = mBonecolor;
			mGuiStyle.normal.textColor = mBoneNameColor;
			Vector3[] tempdiscmBoneVector = mBoneVector.ToArray();
			for (int i = 0; i < tempdiscmBoneVector.Length; i++)
			{
				if (ShowBoneMarker)
				{
					Gizmos.DrawWireSphere(tempdiscmBoneVector[i], BoneMarkerSize);
				}
				if (ShowBoneName)
				{
					Handles.Label(tempdiscmBoneVector[i], mBoneName[i], mGuiStyle);
				}
			}
		}
#endif //UNITY_EDITOR
	}

}   //namespace:BoneViewer
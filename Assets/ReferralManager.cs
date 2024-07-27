//using Firebase;
//using Firebase.Database;
//using Firebase.Auth;
//using UnityEngine;
//using UnityEngine.UI;
//using Firebase;

//public class ReferralManager : MonoBehaviour
//{
//    DatabaseReference reference;
//    FirebaseAuth auth;
//    FirebaseUser currentUser;

//    public InputField referralInputField;
//    public Button submitReferralButton;

//    private void Start()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        {
            
//            auth = FirebaseAuth.DefaultInstance;
//            reference = FirebaseDatabase.DefaultInstance.RootReference;
//            currentUser = auth.CurrentUser;

//            submitReferralButton.onClick.AddListener(HandleReferralSubmit);
//        });
//    }

   
//    void HandleReferralSubmit()
//    {
//        string referralCode = referralInputField.text;

//        if (!string.IsNullOrEmpty(referralCode))
//        {
//            // Check if this referral code exists in the database
//            reference.Child("referralCodes").Child(referralCode).GetValueAsync().ContinueWith(task =>
//            {
//                if (task.IsFaulted)
//                {
//                    Debug.LogError("Error retrieving referral code: " + task.Exception);
//                    return;
//                }

//                DataSnapshot snapshot = task.Result;
//                if (snapshot.Exists)
//                {
//                    string referrerUid = snapshot.Value.ToString();

//                    // Award the referrer
//                    AddCoinsToUser(referrerUid, 1);

//                    // Award the current user
//                    if (currentUser != null)
//                    {
//                        AddCoinsToUser(currentUser.UserId, 1);
//                    }

//                    Debug.Log("Rewards given!");
//                }
//                else
//                {
//                    Debug.LogError("Invalid referral code");
//                }
//            });
//        }
//    }

//    void AddCoinsToUser(string userId, int coinsToAdd)
//    {
//        var userRef = reference.Child("users").Child(userId);
//        userRef.RunTransaction(mutableData =>
//        {
//            var coinsNode = mutableData.Child("coins");
//            int currentCoins = int.Parse(coinsNode.Value.ToString());
//            coinsNode.Value = currentCoins + coinsToAdd;
//            return TransactionResult.Success(mutableData);
//        });
//    }

//    // When user signs up, create a unique referral code for them
//    public void GenerateAndStoreReferralCodeForUser()
//    {
//        if (currentUser != null)
//        {
//            string uniqueReferralCode = GenerateUniqueCode(); // A function to generate a unique code
//            reference.Child("referralCodes").Child(uniqueReferralCode).SetValueAsync(currentUser.UserId);
//        }
//    }

//    string GenerateUniqueCode()
//    {
//        // This is a basic example, you might want to use a more robust method
//        return System.Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
//    }
//}

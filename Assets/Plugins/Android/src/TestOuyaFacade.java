/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package tv.ouya.sdk;

import android.accounts.AccountManager;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.*;
import android.os.Bundle;
import android.os.Parcelable;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;
//import android.widget.Toast;
import org.json.JSONException;
import org.json.JSONObject;
import tv.ouya.console.api.*;
import tv.ouya.console.internal.util.Strings;

import com.google.gson.Gson;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.security.*;
import java.security.spec.X509EncodedKeySpec;
import java.text.ParseException;
import java.util.*;

import com.unity3d.player.UnityPlayer;

public class TestOuyaFacade
{
    /**
     * The tag for log messages
     */

    private static final String LOG_TAG = "IapSample";

    /**
     * The application key. This is used to decrypt encrypted receipt responses. This should be replaced with the
     * application key obtained from the OUYA developers website.
     */

    private static final byte[] APPLICATION_KEY = {
            (byte) 0x30,(byte) 0x81,(byte) 0x9f,(byte) 0x30,(byte) 0x0d,(byte) 0x06,(byte) 0x09,(byte) 0x2a,
            (byte) 0x86,(byte) 0x48,(byte) 0x86,(byte) 0xf7,(byte) 0x0d,(byte) 0x01,(byte) 0x01,(byte) 0x01,
            (byte) 0x05,(byte) 0x00,(byte) 0x03,(byte) 0x81,(byte) 0x8d,(byte) 0x00,(byte) 0x30,(byte) 0x81,
            (byte) 0x89,(byte) 0x02,(byte) 0x81,(byte) 0x81,(byte) 0x00,(byte) 0xdc,(byte) 0xbe,(byte) 0x5f,
            (byte) 0x43,(byte) 0x14,(byte) 0x48,(byte) 0xb1,(byte) 0xb3,(byte) 0x0d,(byte) 0x2f,(byte) 0x7d,
            (byte) 0x69,(byte) 0x02,(byte) 0xda,(byte) 0xae,(byte) 0x19,(byte) 0xcd,(byte) 0x0f,(byte) 0xc8,
            (byte) 0x70,(byte) 0x58,(byte) 0x72,(byte) 0x30,(byte) 0xf5,(byte) 0xd1,(byte) 0x18,(byte) 0xea,
            (byte) 0x98,(byte) 0x3d,(byte) 0x50,(byte) 0x3c,(byte) 0xcb,(byte) 0xb2,(byte) 0x1b,(byte) 0xf7,
            (byte) 0x65,(byte) 0x4c,(byte) 0xb0,(byte) 0x82,(byte) 0x0e,(byte) 0x43,(byte) 0xc4,(byte) 0x67,
            (byte) 0x58,(byte) 0x05,(byte) 0x18,(byte) 0xf9,(byte) 0x45,(byte) 0x20,(byte) 0xcb,(byte) 0x14,
            (byte) 0x4a,(byte) 0xb7,(byte) 0xa7,(byte) 0x55,(byte) 0x83,(byte) 0x45,(byte) 0x6e,(byte) 0x5d,
            (byte) 0x93,(byte) 0xf7,(byte) 0xe2,(byte) 0x5d,(byte) 0x8e,(byte) 0x3b,(byte) 0xf3,(byte) 0x93,
            (byte) 0x6c,(byte) 0x30,(byte) 0xe0,(byte) 0x13,(byte) 0xd5,(byte) 0x21,(byte) 0xf1,(byte) 0x21,
            (byte) 0x90,(byte) 0xa4,(byte) 0xed,(byte) 0x07,(byte) 0x51,(byte) 0x78,(byte) 0x56,(byte) 0xa6,
            (byte) 0xcb,(byte) 0x15,(byte) 0x99,(byte) 0x46,(byte) 0xc4,(byte) 0xb8,(byte) 0xc7,(byte) 0xbd,
            (byte) 0xd8,(byte) 0x1c,(byte) 0x87,(byte) 0x76,(byte) 0xc8,(byte) 0x54,(byte) 0x85,(byte) 0x2a,
            (byte) 0x51,(byte) 0xcf,(byte) 0x5b,(byte) 0xd2,(byte) 0xc7,(byte) 0x3a,(byte) 0xbd,(byte) 0x1b,
            (byte) 0x42,(byte) 0x11,(byte) 0x65,(byte) 0xae,(byte) 0x17,(byte) 0xbb,(byte) 0x55,(byte) 0xf4,
            (byte) 0x58,(byte) 0x54,(byte) 0x9f,(byte) 0xfa,(byte) 0x59,(byte) 0x5c,(byte) 0xbf,(byte) 0xda,
            (byte) 0xfe,(byte) 0xbe,(byte) 0x34,(byte) 0xc6,(byte) 0xc3,(byte) 0x02,(byte) 0x03,(byte) 0x01,
            (byte) 0x00,(byte) 0x01,
    };

    /*
     * Before this app will run, you must define some purchasable items on the developer website. Once
     * you have defined those items, put their Product IDs in the List below.
     * <p/>
     * The Product IDs below are those in our developer account. You should change them.
     */

	public static ArrayList<Purchasable> PRODUCT_IDENTIFIER_LIST = new ArrayList<Purchasable>(0);

    /**
     * The saved instance state key for products
     */

    private static final String PRODUCTS_INSTANCE_STATE_KEY = "Products";

    /**
     * The saved instance state key for receipts
     */

    private static final String RECEIPTS_INSTANCE_STATE_KEY = "Receipts";
	
    /**
     * The ID used to track the activity started by an authentication intent during a purchase.
     */

    public static final int PURCHASE_AUTHENTICATION_ACTIVITY_ID = 1;

    /**
     * The ID used to track the activity started by an authentication intent during a request for
     * the gamers UUID.
     */

    public static final int GAMER_UUID_AUTHENTICATION_ACTIVITY_ID = 2;

    /**
     * The receipt adapter will display a previously-purchased item in a cell in a ListView. It's not part of the in-app
     * purchase API. Neither is the ListView itself.
     */
    //private ListView receiptListView;
    /**
     * Your game talks to the OuyaFacade, which hides all the mechanics of doing an in-app purchase.
     */
    private OuyaFacade ouyaFacade;

    private List<Product> mProductList;
    private List<Receipt> mReceiptList;

    /**
     * The outstanding purchase request UUIDs.
     */

    private final Map<String, Product> mOutstandingPurchaseRequests = new HashMap<String, Product>();
	
    /**
     * Broadcast listener to handle re-requesting the receipts when a user has re-authenticated
     */

	// moved to java application

	/*
    private BroadcastReceiver mAuthChangeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            requestReceipts();
        }
    };
	*/

    /**
     * The cryptographic key for this application
     */

    private PublicKey mPublicKey;

	//android context
	private Context context;

	// Custom-iap-code, listener for getting products
	CancelIgnoringOuyaResponseListener<ArrayList<Product>> m_productListListener = null;

	public TestOuyaFacade(Context context, Bundle savedInstanceState, String developerId)
	{
		try
		{
			this.context = context;

			Log.i("TestOuyaFacade", "TestOuyaFacade.Init(" + developerId + ");");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "DebugLog", "TestOuyaFacade.Init(" + developerId + ");");

			ouyaFacade = OuyaFacade.getInstance();

			Init(developerId);

			// Create a PublicKey object from the key data downloaded from the developer portal.
			try {
				X509EncodedKeySpec keySpec = new X509EncodedKeySpec(APPLICATION_KEY);
				KeyFactory keyFactory = KeyFactory.getInstance("RSA");
				mPublicKey = keyFactory.generatePublic(keySpec);
			} catch (Exception e) {
				Log.e(LOG_TAG, "Unable to create encryption key", e);
			}
        }
		catch (Exception ex)
		{
            Log.e(LOG_TAG, "TestOuyaFacade constructor exception", ex);
        }
	}

	public class ErrorResponse
	{
		public int errorCode = 0;
		public String errorMessage = "";
	}

	private void Init(String developerId)
	{
		Log.i("TestOuyaFacade", "OuyaFacade.init(context, " + developerId + ");");
		UnityPlayer.UnitySendMessage("OuyaGameObject", "DebugLog", "ouyaFacade.init(context, " + developerId + ");");
        ouyaFacade.init(context, developerId);

		// custom-iap-code
		m_productListListener = new CancelIgnoringOuyaResponseListener<ArrayList<Product>>()
		{
			@Override
			public void onSuccess(final ArrayList<Product> products) {
				mProductList = products;

				// custom-iap-code

				// clear the old list
				Log.i("TestOuyaFacade", "m_productListListener ProductListClearListener");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ProductListClearListener", "");

				//send each item in the list
				if(mProductList != null)
				{
					for (Product product : mProductList)
					{
						Gson gson = new Gson();
						String jsonData = gson.toJson(product);

						Log.i("TestOuyaFacade", "m_productListListener ProductListListener jsonData=" + jsonData);
						UnityPlayer.UnitySendMessage("OuyaGameObject", "ProductListListener", jsonData);
					}
				}

				//send the complete message
				Log.i("TestOuyaFacade", "m_productListListener ProductListCompleteListener");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ProductListCompleteListener", "");

			}

			@Override
			public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
				// Your app probably wants to do something more sophisticated than popping a Toast. This is
				// here to tell you that your app needs to handle this case: if your app doesn't display
				// something, the user won't know of the failure.
				//Toast.makeText(IOuyaActivity.GetActivity(), "Could not fetch product information (error " + errorCode + ": " + errorMessage + ")", Toast.LENGTH_LONG).show();

				Gson gson = new Gson();
				ErrorResponse er = new ErrorResponse();
				er.errorCode = errorCode;
				er.errorMessage = errorMessage;
				String jsonData = gson.toJson(er);

				Log.i("TestOuyaFacade", "m_productListListener ProductListFailureListener=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ProductListFailureListener", jsonData);
			}
		};
	}

    /**
     * Check for the result from a call through to the authentication intent. If the authentication was
     * successful then re-try the purchase.
     */

    public void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        if(resultCode == Activity.RESULT_OK) {
            switch (requestCode) {
                case GAMER_UUID_AUTHENTICATION_ACTIVITY_ID:
                    fetchGamerUUID();
                    break;
                case PURCHASE_AUTHENTICATION_ACTIVITY_ID:
                    restartInterruptedPurchase();
                    break;
            }
        }
    }

    /**
     * Restart an interrupted purchase.
     */

    public void restartInterruptedPurchase() {
		//custom iap code
		final String suspendedPurchaseId = OuyaPurchaseHelper.getSuspendedPurchase(context);
        //final String suspendedPurchaseId = OuyaPurchaseHelper.getSuspendedPurchase(this);
        if(suspendedPurchaseId == null) {
            return;
        }

        try {
            for(Product thisProduct : mProductList) {
                if(suspendedPurchaseId.equals(thisProduct.getIdentifier())) {
                    requestPurchase(thisProduct);
                    break;
                }
            }
        } catch (Exception ex) {
            Log.e(LOG_TAG, "Error during purchase request", ex);
            showError(ex.getMessage());
        }
    }

    /**
     * Save the products and receipts if we're going for a restart
     */

    public void onSaveInstanceState(final Bundle outState) {
        if(mProductList != null) {
            outState.putParcelableArray(PRODUCTS_INSTANCE_STATE_KEY, mProductList.toArray(new Product[mProductList.size()]));
        }
        if(mReceiptList != null) {
            outState.putParcelableArray(RECEIPTS_INSTANCE_STATE_KEY, mReceiptList.toArray(new Receipt[mReceiptList.size()]));
        }
    }

    /*
     * The IAP Facade registers a broadcast receiver with Android. You should take care to call shutdown(),
     * which unregisters the broadcast receiver, when you're done with the IAP Facade.
     */
    public void onDestroy() {
        ouyaFacade.shutdown();
    }

    /**
     * Get the list of products the user can purchase from the server.
     */
    public void requestProducts() {
		//custom-iap-code
		if (null != m_productListListener)
		{
			Log.i("TestOuyaFacade", "TestOuyaFacade.requestProducts m_productListListener is valid");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "DebugLog", "TestOuyaFacade.requestProducts m_productListListener is valid");
			ouyaFacade.requestProductList(PRODUCT_IDENTIFIER_LIST, m_productListListener);
		}
		else
		{
			Log.i("TestOuyaFacade", "TestOuyaFacade.requestProducts m_productListListener is null");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "DebugLog", "TestOuyaFacade.requestProducts m_productListListener is null");
		}
    }

    public void fetchGamerUUID() {
        ouyaFacade.requestGamerUuid(new CancelIgnoringOuyaResponseListener<String>() {
            @Override
            public void onSuccess(String result) {
				/*
                new AlertDialog.Builder(IapSampleActivity.this)
                        .setTitle(getString(R.string.alert_title))
                        .setMessage(result)
                        .setPositiveButton(R.string.ok, null)
                        .show();
				*/
            }

            @Override
            public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
                Log.w(LOG_TAG, "fetch gamer UUID error (code " + errorCode + ": " + errorMessage + ")");
                boolean wasHandledByAuthHelper =
                        OuyaAuthenticationHelper.
                                handleError(
										//custom iap code										
										IOuyaActivity.GetActivity(), errorCode, errorMessage,

                                        //IapSampleActivity.this, errorCode, errorMessage,
                                        optionalData, GAMER_UUID_AUTHENTICATION_ACTIVITY_ID,
                                        new OuyaResponseListener<Void>() {
                                            @Override
                                            public void onSuccess(Void result) {
                                                fetchGamerUUID();   // Retry the fetch if the error was handled.
                                            }

                                            @Override
                                            public void onFailure(int errorCode, String errorMessage,
                                                                  Bundle optionalData) {
                                                showError("Unable to fetch gamer UUID (error " +
                                                         errorCode + ": " + errorMessage + ")");
                                            }

                                            @Override
                                            public void onCancel() {
                                                showError("Unable to fetch gamer UUID");
                                            }
                                        });

                if (!wasHandledByAuthHelper) {
                    showError("Unable to fetch gamer UUID (error " + errorCode + ": " + errorMessage + ")");
                }
            }
        });
    }

    /**
     * Request the receipts from the users previous purchases from the server.
     */

    public void requestReceipts() {
        ouyaFacade.requestReceipts(new ReceiptListener());
    }

    /*
     * This will be called when the user clicks on an item in the ListView.
     */
    public void requestPurchase(final Product product)
        throws GeneralSecurityException, UnsupportedEncodingException, JSONException {
        SecureRandom sr = SecureRandom.getInstance("SHA1PRNG");

        // This is an ID that allows you to associate a successful purchase with
        // it's original request. The server does nothing with this string except
        // pass it back to you, so it only needs to be unique within this instance
        // of your app to allow you to pair responses with requests.
        String uniqueId = Long.toHexString(sr.nextLong());

        JSONObject purchaseRequest = new JSONObject();
        purchaseRequest.put("uuid", uniqueId);
        purchaseRequest.put("identifier", product.getIdentifier());
        purchaseRequest.put("testing", "true"); // This value is only needed for testing, not setting it results in a live purchase
        String purchaseRequestJson = purchaseRequest.toString();

        byte[] keyBytes = new byte[16];
        sr.nextBytes(keyBytes);
        SecretKey key = new SecretKeySpec(keyBytes, "AES");

        byte[] ivBytes = new byte[16];
        sr.nextBytes(ivBytes);
        IvParameterSpec iv = new IvParameterSpec(ivBytes);

        Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding", "BC");
        cipher.init(Cipher.ENCRYPT_MODE, key, iv);
        byte[] payload = cipher.doFinal(purchaseRequestJson.getBytes("UTF-8"));

        cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding", "BC");
        cipher.init(Cipher.ENCRYPT_MODE, mPublicKey);
        byte[] encryptedKey = cipher.doFinal(keyBytes);

        Purchasable purchasable =
                new Purchasable(
                        product.getIdentifier(),
                        Base64.encodeToString(encryptedKey, Base64.NO_WRAP),
                        Base64.encodeToString(ivBytes, Base64.NO_WRAP),
                        Base64.encodeToString(payload, Base64.NO_WRAP) );

        synchronized (mOutstandingPurchaseRequests) {
            mOutstandingPurchaseRequests.put(uniqueId, product);
        }

		//custom-iap-code
		Log.i("TestOuyaFacade", "TestOuyaFacade.requestPurchase(" + product.getIdentifier() + ")");
		UnityPlayer.UnitySendMessage("OuyaGameObject", "DebugLog", "TestOuyaFacade.requestPurchase(" + product.getIdentifier() + ")");
        
		ouyaFacade.requestPurchase(purchasable, new PurchaseListener(product));
    }

    /**
     * Display an error to the user. We're using a toast for simplicity.
     */

    private void showError(final String errorMessage) {
        //Toast.makeText(IapSampleActivity.this, errorMessage, Toast.LENGTH_LONG).show();
    }

    /**
     * The callback for when the list of user receipts has been requested.
     */
    private class ReceiptListener implements OuyaResponseListener<String> {

        /**
         * Handle the successful fetching of the data for the receipts from the server.
         *
         * @param receiptResponse The response from the server.
         */
        @Override
        public void onSuccess(String receiptResponse)
		{
            OuyaEncryptionHelper helper = new OuyaEncryptionHelper();
            List<Receipt> receipts;
            try
			{
                JSONObject response = new JSONObject(receiptResponse);
                if(response.has("key") && response.has("iv"))
				{
                    receipts = helper.decryptReceiptResponse(response, mPublicKey);
                }
				else
				{
                    receipts = helper.parseJSONReceiptResponse(receiptResponse);
                }
            }
			catch (ParseException e)
			{
                //throw new RuntimeException(e);
				
				Gson gson = new Gson();
				ErrorResponse er = new ErrorResponse();
				er.errorCode = 0;
				er.errorMessage = "RuntimeException: " + e;
				String jsonData = gson.toJson(er);

				Log.i("TestOuyaFacade", "ReceiptListener ReceiptListFailureListener=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListFailureListener", jsonData);

				return;

            }
			catch (JSONException e)
			{
                if(e.getMessage().contains("ENCRYPTED"))
				{
                    // This is a hack for some testing code which will be removed
                    // before the consumer release
                    try
					{
                        receipts = helper.parseJSONReceiptResponse(receiptResponse);
                    }
					catch (IOException ioe)
					{
                        //throw new RuntimeException(ioe);

						Gson gson = new Gson();
						ErrorResponse er = new ErrorResponse();
						er.errorCode = 0;
						er.errorMessage = "IOException: " + ioe;
						String jsonData = gson.toJson(er);

						Log.i("TestOuyaFacade", "ReceiptListener ReceiptListFailureListener=" + jsonData);
						UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListFailureListener", jsonData);

						return;

                    }
                }
				else
				{
                    //throw new RuntimeException(e);
					Gson gson = new Gson();
					ErrorResponse er = new ErrorResponse();
					er.errorCode = 0;
					er.errorMessage = "RuntimeException: " + e;
					String jsonData = gson.toJson(er);

					Log.i("TestOuyaFacade", "ReceiptListener ReceiptListFailureListener=" + jsonData);
					UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListFailureListener", jsonData);

					return;
                }
            }
			catch (GeneralSecurityException e)
			{
                //throw new RuntimeException(e);

				Gson gson = new Gson();
				ErrorResponse er = new ErrorResponse();
				er.errorCode = 0;
				er.errorMessage = "GeneralSecurityException: " + e;
				String jsonData = gson.toJson(er);

				Log.i("TestOuyaFacade", "ReceiptListener ReceiptListFailureListener=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListFailureListener", jsonData);

				return;
            }
			catch (IOException e)
			{
                //throw new RuntimeException(e);
				
				Gson gson = new Gson();
				ErrorResponse er = new ErrorResponse();
				er.errorCode = 0;
				er.errorMessage = "IOException: " + e;
				String jsonData = gson.toJson(er);

				Log.i("TestOuyaFacade", "ReceiptListener ReceiptListFailureListener=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListFailureListener", jsonData);

				return;
            }
            Collections.sort(receipts, new Comparator<Receipt>() {
                @Override
                public int compare(Receipt lhs, Receipt rhs) {
                    return rhs.getPurchaseDate().compareTo(lhs.getPurchaseDate());
                }
            });

            mReceiptList = receipts;

			// custom-iap-code

			// clear the old list
			Log.i("TestOuyaFacade", "ReceiptListener ReceiptListClearListener");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListClearListener", "");

			//send each item in the list
			if(mReceiptList != null)
			{
				for (Receipt receipt : mReceiptList)
				{
					Gson gson = new Gson();
					String jsonData = gson.toJson(receipt);

					Log.i("TestOuyaFacade", "ReceiptListener ReceiptListListener jsonData=" + jsonData);
					UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListListener", jsonData);
				}
			}

			//send the complete message
			Log.i("TestOuyaFacade", "ReceiptListener ReceiptListCompleteListener");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListCompleteListener", "");
        }

        /**
         * Handle a failure. Because displaying the receipts is not critical to the application we just show an error
         * message rather than asking the user to authenticate themselves just to start the application up.
         *
         * @param errorCode An HTTP error code between 0 and 999, if there was one. Otherwise, an internal error code from the
         *                  Ouya server, documented in the {@link OuyaErrorCodes} class.
         *
         * @param errorMessage Empty for HTTP error codes. Otherwise, a brief, non-localized, explanation of the error.
         *
         * @param optionalData A Map of optional key/value pairs which provide additional information.
         */

        @Override
        public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {
            Log.w(LOG_TAG, "Request Receipts error (code " + errorCode + ": " + errorMessage + ")");
            showError("Could not fetch receipts (error " + errorCode + ": " + errorMessage + ")");
        }

        /**
         * Handle the cancel event.
         *
         */
        @Override
        public void onCancel()
		{
			showError("Fetch receipts was cancelled");

			Log.i("TestOuyaFacade", "PurchaseListener Invoke ReceiptListCancelListener");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "ReceiptListCancelListener", "");
		}
    }

    /**
     * The callback for when the user attempts to purchase something
     */
    private class PurchaseListener implements OuyaResponseListener<String> {
        /**
         * The ID of the product the user is trying to purchase. This is used in the
         * onFailure method to start a re-purchase if they user wishes to do so.
         */

        private Product mProduct;

        /**
         * Constructor. Store the ID of the product being purchased.
         */

        PurchaseListener(final Product product) {
            mProduct = product;
        }

        /**
         * Handle a successful purchase.
         *
         * @param result The response from the server.
         */
        @Override
        public void onSuccess(String result)
		{
            Product product = null;
			Product storedProduct = null;
            String id = "";
            try
			{
                OuyaEncryptionHelper helper = new OuyaEncryptionHelper();

                JSONObject response = new JSONObject(result);
                if(response.has("key") && response.has("iv"))
				{
                    id = helper.decryptPurchaseResponse(response, mPublicKey);
                    synchronized (mOutstandingPurchaseRequests)
					{
                        storedProduct = mOutstandingPurchaseRequests.remove(id);
                    }
                    if(storedProduct == null || !storedProduct.getIdentifier().equals(mProduct.getIdentifier()))
					{
                        onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, "Purchased product is not the same as purchase request product", Bundle.EMPTY);
                        return;
                    }
                }
				else
				{
                    product = new Product(new JSONObject(result));
                    if(!mProduct.getIdentifier().equals(product.getIdentifier()))
					{
                        onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, "Purchased product is not the same as purchase request product", Bundle.EMPTY);
                        return;
                    }
                }
            }
			catch (ParseException e)
			{
                onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, e.getMessage(), Bundle.EMPTY);
            }
			catch (JSONException e)
			{
                if(e.getMessage().contains("ENCRYPTED"))
				{
                    // This is a hack for some testing code which will be removed
                    // before the consumer release
                    try
					{
                        product = new Product(new JSONObject(result));
                        if(!mProduct.getIdentifier().equals(product.getIdentifier()))
						{
                            onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, "Purchased product is not the same as purchase request product", Bundle.EMPTY);
                            return;
                        }
                    }
					catch (JSONException jse)
					{
                        onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, e.getMessage(), Bundle.EMPTY);
                        return;
                    }
                }
				else
				{
                    onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, e.getMessage(), Bundle.EMPTY);
                    return;
                }
            }
			catch (IOException e)
			{
                onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, e.getMessage(), Bundle.EMPTY);
                return;
            }
			catch (GeneralSecurityException e)
			{
                onFailure(OuyaErrorCodes.THROW_DURING_ON_SUCCESS, e.getMessage(), Bundle.EMPTY);
                return;
            }

			/*
            new AlertDialog.Builder(IapSampleActivity.this)
                    .setTitle(getString(R.string.alert_title))
                    .setMessage("You have successfully purchased a " + mProduct.getName() + " for " + Strings.formatDollarAmount(mProduct.getPriceInCents()))
                    .setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialogInterface, int i) {
                            dialogInterface.dismiss();
                        }
                    })
                    .show();
            requestReceipts();
			*/

			if (null != product)
			{
				Gson gson = new Gson();
				String jsonData = gson.toJson(product);

				Log.i("TestOuyaFacade", "PurchaseListener PurchaseSuccessListener jsonData=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseSuccessListener", jsonData);
			}
			else if (null != storedProduct)
			{
				Gson gson = new Gson();
				String jsonData = gson.toJson(storedProduct);

				Log.i("TestOuyaFacade", "PurchaseListener PurchaseSuccessListener jsonData=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseSuccessListener", jsonData);
			}
        }

        /**
         * Handle an error. If the OUYA framework supplies an intent this means that the user needs to
         * either authenticate or re-authenticate themselves, so we start the supplied intent.
         *
         * @param errorCode An HTTP error code between 0 and 999, if there was one. Otherwise, an internal error code from the
         *                  Ouya server, documented in the {@link OuyaErrorCodes} class.
         *
         * @param errorMessage Empty for HTTP error codes. Otherwise, a brief, non-localized, explanation of the error.
         *
         * @param optionalData A Map of optional key/value pairs which provide additional information.
         */

        @Override
        public void onFailure(int errorCode, String errorMessage, Bundle optionalData) {

			// custom iap-code
			OuyaPurchaseHelper.suspendPurchase(IOuyaActivity.GetActivity(), mProduct.getIdentifier());

            //OuyaPurchaseHelper.suspendPurchase(IapSampleActivity.this, mProduct.getIdentifier());

            boolean wasHandledByAuthHelper =
                    OuyaAuthenticationHelper.
                            handleError(

									// custom iap-code
									IOuyaActivity.GetActivity(), errorCode, errorMessage,

                                    //IapSampleActivity.this, errorCode, errorMessage,
                                    optionalData, PURCHASE_AUTHENTICATION_ACTIVITY_ID,
                                    new OuyaResponseListener<Void>()
									{
                                        @Override
                                        public void onSuccess(Void result)
										{
                                            restartInterruptedPurchase();   // Retry the purchase if the error was handled.
                                        }

                                        @Override
                                        public void onFailure(int errorCode, String errorMessage,
                                                              Bundle optionalData)
										{
											Gson gson = new Gson();
											ErrorResponse er = new ErrorResponse();
											er.errorCode = errorCode;
											er.errorMessage = errorMessage;
											String jsonData = gson.toJson(er);

											Log.i("TestOuyaFacade", "PurchaseListener PurchaseFailureListener=" + jsonData);
											UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseFailureListener", jsonData);

											return;
                                        }

                                        @Override
                                        public void onCancel()
										{
											Log.i("TestOuyaFacade", "PurchaseListener PurchaseCancelListener=");
											UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseCancelListener", "");

											return;
                                        }
                                    });


            if(!wasHandledByAuthHelper)
			{
				Gson gson = new Gson();
				ErrorResponse er = new ErrorResponse();
				er.errorCode = errorCode;
				er.errorMessage = errorMessage;
				String jsonData = gson.toJson(er);

				Log.i("TestOuyaFacade", "PurchaseListener PurchaseFailureListener=" + jsonData);
				UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseFailureListener", jsonData);

				return;				
            }
        }

        /**
         * Handle the cancel event.
         *
         */
        @Override
        public void onCancel()
		{
			showError("Purchase was cancelled");

			Log.i("TestOuyaFacade", "PurchaseListener Invoke PurchaseCancelListener");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "PurchaseCancelListener", "");
		}
    }

}
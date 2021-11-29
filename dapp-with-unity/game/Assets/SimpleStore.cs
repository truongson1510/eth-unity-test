
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Hex.HexTypes;

public class SimpleStore : MonoBehaviour
{
    [Header("Deployed contract")]
    public TextAsset contractABI;
    public TextAsset contractAddress;

    public InputField inputValue;
    public Text currentValueText;
    public Text addressText;
    public Text balanceText;

    private HexBigInteger ethBalance;
    private Web3 web3;
    private Account account;
    private string privateKey;
    private string from;
    private Contract contract;

    private HexBigInteger gas = new HexBigInteger(900000);

    private Function getFunction;
    private Function setFunction;

    void Start()
    {
        AccountSetup();

        //Debug.Log("contractABI: " + contractABI.text);
        //Debug.Log("contractAddress: " + contractAddress.text);
    }

    public void AccountSetup()
    {
        var url = "http://localhost:8545";
        privateKey = "0x59ec7694bf44e793c3f5fbd9d3b3ca4e0852c0cdade6c21c0037b88ae5d7961f";
        account = new Account(privateKey);  // Type Account
        from = account.Address;             // Type string
        web3 = new Web3(account, url);      // Type Web3
        StartCoroutine(StatusInterval());
        GetContract();
    }

    IEnumerator StatusInterval()
    {
        while (true)
        {
            UpdateStatus();
            //Debug.Log("Updating . . .");
            yield return new WaitForSeconds(1f);
        }
    }

    public async Task UpdateStatus()
    {
        uint newValFromContract = await GetValue();

        Debug.Log("newValFromContract = " + newValFromContract);
        currentValueText.text = "" + newValFromContract;



        var newBalance = await web3.Eth.GetBalance.SendRequestAsync(from);  // from = account.Address;   
        ethBalance = newBalance;            //  Type HexBigInteger
        decimal ethBalanceVal = Web3.Convert.FromWei(ethBalance.Value);
        addressText.text = from;
        balanceText.text = string.Format("{0:0.00} ETH", ethBalanceVal);
    }

    public void OnSetValue()
    {
        var newValue = inputValue.text;
        try
        {
            int intValue = Int32.Parse(newValue);
            SetValue(intValue);
        }
        catch (FormatException)
        {
            Debug.LogError("Unable to parse input!");
        }
    }

    void GetContract()
    {
        string abi = contractABI.ToString();
        string address = contractAddress.ToString();
        contract = web3.Eth.GetContract(abi, address);

        getFunction = contract.GetFunction("get");
        setFunction = contract.GetFunction("set");
    }

    public async Task<uint> GetValue()
    {
        var value = await getFunction.CallAsync<uint>();
        return value;
    }

    public async Task<string> SetValue(int value)
    {
        var receipt = await setFunction.SendTransactionAndWaitForReceiptAsync(from, gas, new HexBigInteger(0), null, value);
        Debug.LogFormat("tx: {0}", receipt.TransactionHash);
        return receipt.TransactionHash;
    }
}

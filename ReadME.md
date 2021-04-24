Transaction Discovery Service (TDS)


We need to build an distributed application written on C# (.NET Core) which discovers wallet transactions. 
You can implement interaction with API in the way that is the most convenient for you (Blazor, Console, Postman, etc). 
The application should be written as production-ready.
Create as many services as you think are appropriate.
Use whatever database you think is appropriate.
Use any libraries/tools you think are appropriate. 
Coverage with unit tests will be a plus.

-----------------------------------------------

TDS API exposes the REST endpoint to insert/upload a list of Stellar wallet addresses.
This endpoint starts to discover all the payment transactions associated with this wallet (consider transactions with type Payment and consider payments only within native currency XLM).
TDS calls Stellar ledger to get transactions and persists them into the database.
Every wallet may be discovered several times. All new transactions should be recognized and persisted. 


*********************************
**** OPTIONAL PART ****

TDS API exposes the REST endpoint to download CSV report for a provided wallet address.
The report should contain the transfer history with aggregated transferred amount grouped by source/destination wallet address.

The format should be as follows:
Wallet,Amount

-------------------------------------------
Example:
Let's assume we have 3 wallets with ids: W1, W2, W3.

W1 transfer history:
W1 sent to W2: 10 XLM
W1 sent to W3: 10 XLM
W1 received from W2: 5 XLM

the report generated for W1 should be as follows:
W2,5
W3,10
***************************************************

Test wallets: 
GCPWZSMOLI7SWZWQYILSPZIPJMB3ZOR5JNB6DH2OPAPLPAD2BHMRP2VT
SANWEQERPWZSCLFLQVKFX4PIMAVJHSJ5MMDQMGTSYSRPDSBEKPWPLEQS
 
GD7Q3Q2AHVAYGKTKII3O5LQMYZJWB24ITLINSAFTJHPVX2S6N7XTVJII
SBLQJYBVKNV3M3PCSGEIHBFUEY2AE4PP6RFXQ66AXDQFA3OEWT77CXSL
 
GCIMYSPNPJHHUVGCIHJYFN7BFUBERQSLHV5TZYVU4QR7RYTSZSQBX7SN
SDBVSZGWZTPS2FWBYJA73C3W6VUQU2TUKG4MMD2G7VIET7FUE43MD7CM
 
GCYY337UP2VNQTJ4AZTO2K54HK5V5RARB6BDFQXLFVZMEKFPTZTWBJQP
SDJCGE6RA7UGHCQ5DHRIQ2YADM32KVWAPCNSFVRNVRTUPOVTW5ZJAZKN

---------------------------------------------
Stellar API laboratory:
https://laboratory.stellar.org/#explorer?resource=payments&endpoint=for_account&network=test

Stellar C# SDK: 
https://developers.stellar.org/docs/software-and-sdks/#c#-net
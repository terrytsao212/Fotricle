ALTER TABLE [dbo].[OpenTimes] ADD [OpenDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FeedBacks')
AND col_name(parent_object_id, parent_column_id) = 'Food';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FeedBacks] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[FeedBacks] ALTER COLUMN [Food] [int] NOT NULL
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FeedBacks')
AND col_name(parent_object_id, parent_column_id) = 'Service';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FeedBacks] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[FeedBacks] ALTER COLUMN [Service] [int] NOT NULL
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.FeedBacks')
AND col_name(parent_object_id, parent_column_id) = 'AllSuggest';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[FeedBacks] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[FeedBacks] ALTER COLUMN [AllSuggest] [int] NOT NULL
DECLARE @var3 nvarchar(128)
SELECT @var3 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.OpenTimes')
AND col_name(parent_object_id, parent_column_id) = 'SDateTime';
IF @var3 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[OpenTimes] DROP CONSTRAINT [' + @var3 + ']')
ALTER TABLE [dbo].[OpenTimes] ALTER COLUMN [SDateTime] [datetime] NULL
DECLARE @var4 nvarchar(128)
SELECT @var4 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.OpenTimes')
AND col_name(parent_object_id, parent_column_id) = 'EDateTimeDate';
IF @var4 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[OpenTimes] DROP CONSTRAINT [' + @var4 + ']')
ALTER TABLE [dbo].[OpenTimes] ALTER COLUMN [EDateTimeDate] [datetime] NULL
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202009191544267_changeOpentimetype', N'Fotricle.Migrations.Configuration',  0x1F8B0800000000000400ED5D5B6F2447157E47E23F8CE609D0C6637B132E2B1BE478636411AFCDCEEE8AB7A83D539E6DA5A77BE8EED9AC857801718940649120808010044140048180202808FE4CBC9B7DE22F50DDD5D575AFAEEAAEEE99762CBF78EAF255D5A9734E559D3A5DE77FEFFF77E70B0FE7C1E00188133F0A77875B1B9BC3010827D1D40F67BBC3657AF6CC67875FF8FCC73FB6F3C274FE70700F97BB9E958335C36477783F4D173746A364721FCCBD6463EE4FE22889CED28D49341F79D368B4BDB9F9B9D1D6D608408821C41A0C766E2FC3D49F83FC07FCB91F8513B048975E70144D419014E930679CA30E6E7973902CBC09D81D1E4469EC4F02B0818A0E077B81EFC16E8C4170361C786118A55E0A3B79E36E02C6691C85B3F1022678C19DF30580E5CEBC200145E76F90E2A6E3D8DCCEC631221531D46499A4D1DC1270EB7A4198115FBD16798725E120E95E80244ECFB351E7E4DB1D3E1F7BE17438E05BBAB11FC4592981B41B79856B039C7CAD9C7FC826D9DFB5C1FE32489731D80DC1328DBDE0DAE064791AF8932F81F33BD1CB20DC0D974140770A760BE6310930E9248E16204ECF6F83B3A2AB87B09F23B6DE88AF5856A3EAA0711C86E9F5EDE1E0166CDC3B0D4039E7D498C76914832F8210C45E0AA6275E9A8238CC30404E35A175AEAD17E69E1FE0E620974169190E8EBC872F827096DEDF1DC239190E0EFC87608A538A2EDC0D7D285CB0521A2F81A48BFA664FBC2479258AA79A96B7DA6D79EC05A9A675F8AF51EBFAC672BECBFED5B4F49CD9300D1ACA58E1DCC15C5610F07E14825BCBF92988DB1ED5DE741A832471C024FA76EEF98BB20DA87C37E0EF23800668C95DE3284E1928983F5D4E52946E09F662348B20B5D3A8753EDDF7E2C3B937D3B1A99B86C6506B2D138642C70B101EC7B7A21AF4F14370E2E938DE4DA7BF1CEFC3B2AD377370BA379944703FD1BD5284CBA17FE64F8AF59A960426C356182670696A5B471C867E7A132E7DB89DECFF3BFEDC6891DC8FE68B254CA85479209EFB494213C7D5686E790FFC59DE49AECD4C2CB2714061B90D82BC4472DF5F504293E5BE546C850EE2687E3B0AA87A28E7A53B5E3C03194745D2EC71B48C275CAF764664C3A5DD86612C9B9D18AE73B51993B495CF89D0A0BE4E465039FF5B4A2B0DD20FFD3F2663E5C75EB1E9C505E584AB5A95393DD927C5A65437851E69A26AB02E51A81AAC896AA91AB841496DD44C56FE4AC548DADACF4908E2EA362B9644B4AF7DD14FD2A65072AD5707A4E2A4E546851523EF66635E34D6E5C0609DD4096F9C40D903CD90F6E6F47EB81E867B3D6AAEB50A61B3D25C459D2BED2569ABCA5A65B8F05A72D0DD04C45D18713E224631C8E1B90DA9F586209F4D292355B69941495982BD2EEAC03C8248D3C1BAB2770A974BED1ED6910134F0262F077093C0CC0295DA1F657E00C0F479D86F1B658EEB5C297399782E7D9DAA73C380C7F154B2D9ED668F7C10450D11C6207ED07C131504E3E56C0692861B2978A6E2705A33C8777BF625DB34C9F1178BF04BA41439010B99C221582C213B076B0D81F1B4AA6B451149BFF21C75A75076A39379D1B6850530AB70A510256DD531FF297595815A9419E3E8744BFD90D7959BE36A10CDD836EF9DCF41C85DBAA1B42390DECF34709D71545E70BA599E8E8017B04DD56733538AB9385467576F0BEFFC5E07241AFB945D3A9B5D9460BB7841E204A09EB9F836987BF1CBED5F43A376B63A6A67BBA376AE77D4CEB3976C57A2B1C8E7ABB6688E27C9A22D9ECAABB501B90952CF0F149791A480B8151132C52B49A144B35B490267BD2D41D5AE362792B6EA1CA47497046B67F77660AE7666415F4FBB77BD538AA576906A2EA9FE30ED1AC585951D64CACABB4915D176962ED7E884C5F4C95CA151D5AE149AA4AD3AA72DC69BCF91979F99AA6BE766C381CEA3955D03155DF38AB11363F91DC871819395016F4BBABEDFB9E92713072B4A7237618F62F9EFB55E97D4BB685A3F0B7B69215350F56289464AFEE8FC200A82E8151B0D8FEB5CA977495B75D47B67EEFA35AD76EB223A98F144B9617304A1E1B26D77725A437D892D33D40B99EAAE690DF5C6F27C2B4AF3A5D55C9A518D2B5976272E758EAC6D18C6ABED876E1C4693DBC09B32BDCE12EAF9D6AED10D1C120CA95873598250F3F9EEEEDE0A64E1404BA7ABBAE3E0D6ED9EBFB0BE78C375AE548CA42D274EA89CDE68EFD22849BAF8580BE9ADD69B59975D0D960F7157C3E60862CD65DB09F672CE8A35FEF0F0303908BC19F9CCDB4ACE11487341871309C7159CC389A7E58225356AAC18C1C5A3DF0D07F7BC6009FFDF14668529FAF8A7EF9445B7F4452FDEF9C3E337DFFBE0BD3F9515B645FA224A6AA8CB5869EAD397825905859FBCFAAF0F5F7DF7E22FAF5D3CFAA629A99FFCE8174FDF7FC394DA4F7FF3EEE3D77FA622355FFA83F7DE7FFA9B5F95A5AF57F4E41BEF3D79F50F65E9672B4AFFFD4D1AFBB98AD2FFF9C1C5BF5F2B4B7FBA82FD7EF2165DFA3315A5FFF617BAF4676BB01FF5A1537DE62B4156C17A8F7FF1C727DFF9D5E3B7FE64CC7779695A6EB76A108EFD08B58162A470FAAB1B8D4846FB39D72718415989A2FBD13F4DC975F1D6DF8C97926FFDE383F75F6FB48E50EECBF5695B825C724E640ECE0DF41E815905C13EFCFD372E5EFFF3873FFBF19347DFA6D55905F150B5C7DF7DF4E4EDEF993228AA73F1DBBF3EFEF14F4CD76054E7E96F1FC155FEE2CFDF870D9AAEC7456B6C9D8A55F9E2E76F3C7DEB7B59B57FBE7BF1EBB72FDE795BB5429B6DCB586FBD061B331A68155C523E4861B83ABEF69FA7DFF96123E942BE70F54996D55F05A59EFEF20D38FA8B37FF6E412BBA741D5AE59755F54905AB5F72554D590BEB93A904B96CC4DA4B9268E2E7A3A736F4D497F16C632F84D381FE3379CAB85C3EA47104E9E22F2025FCF47C77F82961084AD4D2A98EA016660C16727363435C7F6E8333106776322FD88FC204CE881FA6A24DCF0F27FEC20BB41DE06A19DA02339A97F87CCE4DB0C8F6A161AAA5A749C3A5914F6CBD6C84B3515691666744F1859E5DC42F495473ABF9AC844C2FF91ECD9C69D45FA350C0C4D0CE026FF134DE390E6F82CCA97AB037410F05EE7BC9C49B8A163F285053072CA7EC7E075CA79C1393B6E90BAC95F01E6DD8546A14998993BB01B3D250129B6887EA496CBD0BDD24D270FD1593E8A1A99D52B9BB26C728D8D3C9925DE48EE2154CE89869647DE88A7564B43569BBBCEA5E090389AE54AA49D6F85509DE94C8E1D59C81D41E59DD691D651F3A6020256DFBA581E8D937D115527F6CA7DA48E6C96DC6AEED6826497F3AD64F129A9BF480FB1E64B5BB70FD4AA7FA82DAD1FE7B550B9CBC035D6E9F7BB6B471AE8EAA8955F93D9289250EBEE6ECA27097EC6E399377A0037691D373FD1732D109B5726E7567FC464C53E78CDF02EBACE0ACAEA46D2FCEEABCBFA36AA295CE8F649AB117B239F7A83C263BE51D45273AE01C054DFBC437FA7D8DDC3FD501C7AC6A47236BBE3B46E9D96E867371544DA9CADF914C2AF13F366714859B64D56E66F5666779C73B6032F93CACDB2E0879B6C23AF0681896F76BF975E096E4E6F06E028ACBC3A4F00FE61926831B83941E4A321C10075A965B047E63ABE3DB221902B977AB00C99E939601A067A9AB2A170B8314A05C4B2B40F0F94906424EA255B4C898494A0824CB26D5CB8740E420D8D2520145590264508C49A5020A6FF6643864FF5C0182D4B90C022F88150058546510445972209440890C8BDDD0A942F217D97909AFBC792EFBCE4887A0292AEF9A291C2CA4BC02664768307AC9737B22012A6E52E52617619F4C759F922E0D19D4B7A7141425ED8D89C17C8E20E103D5522E5A32351C5070AD6EFA25AB761B732F7929443168F56595D288CBEE787902945AAD8A0CD20BAA4A72D62086E4F374911815172F86572F54F759BDAC2186FAB2A56DCE60D6063D7F28AF12AC2E139AF28AECFAC090E44D14A85282741670131B787DBDD9BED4F05FA68BA3D719744D4CBA54AFA99D8766F40A236E1B6222F9045E430083C5536DA4AC4F86AE164FE1B36191145A4B9B91AD8DEA7BB985D41041655D6B97044A4DA0B61855DB8CEA8DBC7D0DC07FC5298E5A670431318350BDA676FD9AB12B0C1F4D3400FE9EB43C7597793B231469B848D819294212EF1C798B851FCEA810C545CA608CE213EF3F33B68FDD3B4718A3492209E15BF6B66C298D626F06B8DCECB3DE2938F0E324FB2ED83BF5320FE4FDE95C2856D818142733DC0863461067091FD670F1ECFF622995451196985F8A9A077028D9F707F9A8003FBB62BD411614DA0BBC58F2D9FB7E142CE7A1DA88A4AE5D8448A1018A24730C1288848621A9F64828B0880C0DE59823526FF4D07054B22556113557002BD22DC64A07C665864A6798E395D16F69AC32D11C278F6E4B63E409E6F5D1F7D234004A3147A002D9D23054B2391609554B4391548B71151FA531232BD22CC686BFF961468613CD717090591A06A799A350316469202AD9826F98CF6E190662722C288E02C232044749E618E4B508464796A916F24A457565C4954A17D176469CBA170CECC292225C4AB00B94D1F2450C754D56B0D2E06DBF88A9ABB6B38E9537188262B6C3392EE3A3D24024D51C4944B14570A371A8B0A70C90EA397B1D16170995D935B05936DA5EA633486A9BB2BE22E944F7434D2433BF49B2974A79B5762492767060D65E8DE3831A8DF3056514B0DE4D548DE94A67B8DC63B22FD54A8669BD033AA11F0C9600D6EC217AC857828732EC478CDE16968D3896DCA8E910F173E8CC46786EBB9FE98F36294D408D348AC2AA65A2559455D7F7CC4A8274D23024F5729C7E49EC4C4E0317A9E6483838268D83D32C24933F88EDD99DC148C04B713C765AB18C68C9F406275AE87EF2B80BA3FB49F225D438E41EA789C629FD50EC358EBA6A3B1A07C59E64783F4FB1385760473EE658A1F2EE53E3B8DD55A19093CCD13F4FB1385DE09093CCD902275AC82315759211492ADDCAEA2345A3D32FA15C16970B8D8EFCB2CB1D93F3BEBCDE7A1FF6DD0A13F3749520E6F607F7BC9A7870A7926DB61645D843765751245AF64966B46632CCF1E88086341C9DDEF5C69F0E56C85B6971BA8582F479C581522CB8948945C8F02993638E58BE304E839589B6385B321CC15DBF1A675B8623BC64568D735D8623BC6B568DF3AC0C4778EB4C87D3AF85033B2A355E3E0AA7DE9A8B88AA763B4B89AB7D591BF6A9962C370E2D2DAE6D411F31CB0DE3CBD744EC349E8B0662A7ADBDDE3BB813FA8D6E0903DA5E3DBB173989AC590A99285D754DACEE8CCA458C331AAA48B2EE15FE0444D22DF9D7213A4C12B98CB9782B532D94088A5EC6B0384ABA848A88F86436D142E5E733F62A485D75BDF58FCBAB1FB7A7D1DEF01E76076DC279C55757F67CA7AAD8872B51575B57D7760B57A74B1CA08A55C128ED12CA01710E6E2209E5C783F6B2A0AEBADE3AD88D9B4C19258935431589B6DC2F63FE1E73ADE03FCE17295B2F52CADFA5FF78E1BBCD3895E7A3CF5CC4F35127851F39EFCC8D8A0C0790440FFC69E6C83D3E4F5230DFC80A6C8CBF1AEC077E6EC6C4058EBCD03F03498ADE861E6E6F6E6D0F077B81EF25C8A5BF7053BFC17F6B6EE4B7BE753DF35B07D3F988AF6EEFFD9EA124C994317688C1DB648F2F741044CDCF485A1926CD32E459714F8F5A081F78F1E4BE173341C2B6B390C6B6E192CBCB760DF056036074EFCE827F62EE3DFC248D6812BC4C08192BEDEA739BB5700B177473E29A2033DEE84EFB5CFAA69B4F9A096CEEAE4EB1B06D503E2A587B9DFA9497BA037E218EEA0EC0D87883B506871DD51DF4067BAB3B80A2FCD51D2B00D675BD015721BF75A7F2C307429CC2FFD3FC26A832FAA1A50AA03CDC1B8CC0385AA9DC77BCB76B1E172B346F4378E6E6309C8287BBC3AFE5756E0C0EBFF25251EDDA20DF94DF186C0EBE6E3B71C46D9DE710CB21D0202B564194DFBA7A50265DE11CD69B811177F53E88B8B1248AFEE2BD95423128781DEEE3EEF2EA03C994421D08D95EB29664B23701EE005D7710DD7D349EC1E216A63E0EBE96AC8FB072E196BF18DA5701AF3E5A3E67BFFF230EE14EF57A7F0EACC457DC011876186F20758E8E42C465DC0158E936EEC240401CC7FBAB59E4EED9BDD52CC8D7DBC1E4969733762781A25A93938062F763D038A9A96ADF8886C8AFBCC1B61FBB9437D01D942B797D14DA85DCADE16AD5522B7B09BAAF22BBBA3377E539C35851343F2B53AEE20D58CA70E351F89237ECADDCD25C4BDBD23EE4D5AC567BE4CD0F02B463B90B1B8B9F36D293AC577933BB4879EDEF5655963EE66DC06EB7037BBD1DD8672FE532247302EFED62B4BA6D9FDA5C65D03E53D9412F9C9BAA9A9A701C5994FA6F093A513980F756E456B7FF3BA11DCF1BF3A6A5EDC910BAA1DCD002A314642B63AC33534CE184DE98EED8F3DCB1798E38A2375016C80BBDBFDA46EEEB7DA56AAC554D5BCE446D1A8BFAB003953985F7963D573D99ABDBFC3A32A2981C616BDCEC178EECFD55E37277F1DECA493B6ADCCCB6CDF068439B57E1B0EE000B7BAD3B80EA98572511C3353190A8A7D78B47E051272D23E68AF18FD8B88665625508BC5A41E7548F8D9BC8DEC02AAEB7FA4D58B1ADD546D7AD88FB44CD1889FA40CD982A70A9D3088362D813A60FE651C36B718DE6D13FE78CA379EE4B6C6BE5D1514DE25E3AD216F194633CC350996BAE27145FA1AD9B92A80810663F5126D3CD9A17C47088AD4CBDF98CB8987AE523306273AB0D724B193F3B9177DAD84A6331E9FD967DA53D59D1DCFA6800B525BCD104AEAD4EB09DA98E3503777FB5DA5D6417ABC32A36819DAD0B563BC0D52E0ABAB07FCE5704127698022289FD5E0B348FC2ACDB425011EAB0EE79710D67BFCB739F1503ACFCDCA78DF1D81207E098E114124EEAFDEC2B5FE659DFB9EF629DEF7ACA3B5BE32DE67BB52BBC2EACA7FD0ADF924DB0BC63A13B4012FBBD33D03C55B4DA9D01F7D64DE916CB0719E567AD783E87EE310EDB2AF20F7AD56677383DCD3C5FD00D8722AE2B8F4CAE1204709225C3C7B9D54DE41FFE8AF02859068D425A55C2960BA4085D6649E195618EF926C86149688264C99A50062717E98FA44F243E4A97525E1EAF588A8C0FFC727C9CAB6C45116F5E688B3157086D31B9B2B6A802D56D91EDACD010C992B5A20C162E3481974FA1019C21835784A216C089CA15E04996AC0165C46779346AEE9A52128D5A7791295BB8A8F7DD8A144181CA2E2EA96A24915FB8AC836D8B1772921156DCDA2976E1548749A266A8BC9A60C3CA341F6A455071B388E2B5E690E6532A3487A321313725AA81A9AF53EA74556D2AE56BCADF0BAE3154F14E4032D48A8B83C6332951D0C273DD6E679541D6CFADC670DC6C082B986DCEC2ABD3496DB074DBAA88B3654A86A7B3763666637EED67DF7B76383CDDA25261DA73B4A8B43D54DE382519A8D67EE56898EC568B7E47D9D9109592A8B6D13496C33687C5591B2403D3D9231ACB20BF7F65DFFB351F9EF00E6C99B73342DBDF2201FE14DE7B8527F2659879FDA15F3741E2CF08C40EC40CC184398B97650EC3B308DB04B81EE122822364EA4DE1417D2F4EFD332FFB582C821BFDC40F67C3C13D2F5882ECC19853303D0C8F97E96299C22183F969704E1323332DE8DADF19097DDE395E64BF12174380DDF43347C9E3F0F9A51F4CCB7E1F883EA52A88CC66513857667399664E96B3F312E996100454055490AF34B5DC01F34500C192E370EC3D0075FA7637012F829937393F299EED5583544F044BF69D9BBE378BBD79526090FAF027E4E1E9FCE1E7FF0F37BD865204000100 , N'6.2.0-61023')

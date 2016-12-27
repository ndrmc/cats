UPDATE [dbo].[BusinessProcess]
SET CurrentStateID = (SELECT TOP 1 bpState.BusinessProcessStateID
                            FROM [dbo].[BusinessProcessState] AS bpState
							WHERE bpState.ParentBusinessProcessID = BusinessProcessID
									and bpState.DatePerformed = (SELECT MAX(bpState2.DatePerformed) FROM [dbo].[BusinessProcessState] bpState2 
																	WHERE bpState2.ParentBusinessProcessID = BusinessProcessID))